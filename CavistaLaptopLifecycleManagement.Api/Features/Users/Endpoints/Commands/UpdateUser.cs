using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Commands
{
    [Handler]
    [MapPut("/{userID}")]
    [MapGroup<UserMapGroup>]
    public static partial class UpdateUser
    {
        public sealed record UpdateUserBody
        {
            public Role Role { get; init; }
        }

        public sealed record Command
        {
            [FromRoute]
            public required Guid UserID { get; init; }

            [FromBody]
            public required UpdateUserBody Body { get; init; }
        }

        public sealed record UpdateUserResponse
        {
            public Guid? userId { get; init; }

            public string Message { get; init; }

            public UpdateUserResponse(string message)
            {
                Message = message;
                userId = null;
            }

            public UpdateUserResponse(Guid id)
            {
                Message = "successful";
                userId = id;
            }
        }

        private async static ValueTask<Results<Ok<UpdateUserResponse>, BadRequest<UpdateUserResponse>, UnauthorizedHttpResult>> HandleAsync(
            Command command,
            AuditTrailService auditTrailService,
            CLMDbContext context,
            UserService userService,
            CancellationToken token)
        {
            var user = await userService.GetCurrentUserAsync();

            if (user == null)
            {
                return TypedResults.Unauthorized(); ;
            }

            var existingUser = await userService.GetUserAsync(command.UserID, context);

            if (existingUser == null)
            {
                return TypedResults.BadRequest(new UpdateUserResponse("No user found"));
            }

            var requestBody = command.Body;

            existingUser.Role = requestBody.Role;
            existingUser.Modified = DateTime.UtcNow.ToUniversalTime();

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrailAsync(user.Id, AuditTrailService.AuditAction.Update, AuditTrailService.AuditOn.User, existingUser.Id);

                    return TypedResults.Ok(new UpdateUserResponse(existingUser.Id));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest(new UpdateUserResponse("An error occurred"));
        }
    }
}
