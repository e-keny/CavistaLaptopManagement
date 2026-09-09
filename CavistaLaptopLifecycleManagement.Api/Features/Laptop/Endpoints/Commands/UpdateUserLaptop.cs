using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Endpoints.Commands
{
    [Handler]
    [MapPut("update/{laptopId}")]
    [MapGroup<LaptopMapGroup>]
    public static partial class UpdateUserLaptop
    {
        public sealed record UpdateLaptopBody
        {
            public Guid? UserID { get; init; }

            public required UserLaptopHistoryStatus Status { get; init; }

            public string? Comment { get; init; }
        }

        public sealed record Command
        {
            [FromRoute]
            public required Guid laptopId { get; init; }

            [FromBody]
            public required UpdateLaptopBody Body { get; init; }
        }

        public sealed record UpdateUserResponse
        {
            public Guid? LaptopId { get; init; }

            public string Message { get; init; }

            public UpdateUserResponse(string message)
            {
                Message = message;
                LaptopId = null;
            }

            public UpdateUserResponse(Guid id)
            {
                Message = "successful";
                LaptopId = id;
            }
        }

        private async static ValueTask<Results<Ok<UpdateUserResponse>, BadRequest<UpdateUserResponse>, UnauthorizedHttpResult, StatusCodeHttpResult>> HandleAsync(
            Command command,
            UserLaptopService userLaptopService,
            AuditTrailService auditTrailService,
            NotificationService notificationService,
            CLMDbContext context,
            UserService userService,
            CancellationToken token)
        {
            var requestBody = command.Body;

            if (requestBody.Status == UserLaptopHistoryStatus.Assigned && (requestBody.UserID == null || requestBody.UserID == Guid.Empty))
            {
                return TypedResults.BadRequest(new UpdateUserResponse("User Id must have a value"));
            }

            var CurrentUser = await userService.GetCurrentUserAsync();

            if (CurrentUser == null)
            {
                return TypedResults.Unauthorized(); ;
            }           

            if (!Enum.IsDefined(typeof(UserLaptopHistoryStatus), requestBody.Status))
            {
                return TypedResults.BadRequest(new UpdateUserResponse("Status does not exist"));
            }

            var existingLaptop = await userLaptopService.GetUserLaptopAsync(command.laptopId, context);

            if (existingLaptop == null)
            {
                return TypedResults.BadRequest(new UpdateUserResponse("Laptop not found"));
            }

            var existingLastLaptopStatus = await userLaptopService.GetLaptopLastStatusAsync(command.laptopId, context);

            if (existingLastLaptopStatus == null || (existingLastLaptopStatus.UserLaptopHistoryStatus != requestBody.Status))
            {
                if (requestBody.Status == UserLaptopHistoryStatus.Assigned)
                {                  
                    var userId = requestBody.UserID.HasValue ? requestBody.UserID.Value : Guid.Empty;

                    var existingUser = await userLaptopService.GetUserAsync(userId, context);

                    if (existingUser == null)
                    {
                        return TypedResults.BadRequest(new UpdateUserResponse("User not found"));
                    }

                    var existingUserLaptops = await userLaptopService.GetUserLaptopsAsync(userId, context);

                    if (existingUserLaptops.Any())
                    {
                        return TypedResults.BadRequest(new UpdateUserResponse("User currently has a laptop, please first unassign the current one"));
                    }

                    existingLaptop.UserId = requestBody.UserID;
                    existingLaptop.UserLaptopStatus = requestBody.Status;
                }
                else if (requestBody.Status == UserLaptopHistoryStatus.UnAssigned)
                {
                    existingLaptop.UserId = null;
                    existingLaptop.UserLaptopStatus = UserLaptopHistoryStatus.UnAssigned;
                }
                else
                {
                    existingLaptop.UserId = null;
                    existingLaptop.UserLaptopStatus = requestBody.Status;
                }
            }
            else
            {
                return TypedResults.StatusCode(304);
            }

            var lapTopHistoryToAdd = new Database.Entities.LaptopHistory
            {
                UserLaptopID = existingLaptop.Id,
                UserLaptopHistoryStatus = requestBody.Status,
                Comment = requestBody.Comment,
                ActionBy = CurrentUser.Id,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            context.LaptopHistories.Add(lapTopHistoryToAdd);

            await auditTrailService.AddAuditTrailAsync(context, CurrentUser.Id, AuditTrailService.AuditAction.Update, AuditTrailService.AuditOn.Laptop, existingLaptop.Id);

            if (requestBody.UserID.HasValue)
            {
                var notificationMessage = $"{existingLaptop.AssetName} has been assigned to you";

                _ = Task.Run(() => notificationService.NotifyUser(context, requestBody.UserID.Value, notificationMessage));
            }

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {                   
                    return TypedResults.Ok(new UpdateUserResponse(existingLaptop.Id));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest(new UpdateUserResponse("An error occurred while trying to update a laptop"));
        }
    }
}
