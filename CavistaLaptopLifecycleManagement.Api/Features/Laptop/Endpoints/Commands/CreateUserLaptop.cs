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

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Endpoints
{
    [Handler]
    [MapPost("create")]
    [MapGroup<LaptopMapGroup>]
    public static partial class CreateUserLaptop
    {
        public sealed record CreateLaptopBody
        {
            public required string AssetName { get; init; }

            public required string Model { get; init; }

            public required string Comment { get; init; }

            public required string AssetLocation { get; init; }

            public required string EmployeeDepartment { get; init; }

            public required decimal Price { get; init; }

            public required DateTimeOffset EstimationUsefulLifeYear { get; init; }

            public required DateTimeOffset DepreciationEstimationDate { get; init; }

            public required DateTimeOffset WarrantyExpirationDate { get; init; }

            public required DateTimeOffset PurchaseYear { get; init; }
        }

        public sealed record Command
        {
            //[FromRoute]
            //public required Guid UserID { get; init; }

            [FromBody]
            public required CreateLaptopBody Body { get; init; }
        }

        public sealed record Response
        {
            public required Guid LaptopId { get; init; }
        }

        private async static  ValueTask<Results<Ok<Response>, BadRequest, UnauthorizedHttpResult>> HandleAsync(
            Command command,
            UserLaptopService userLaptopService,
            AuditTrailService auditTrailService,
            CLMDbContext context,
            UserService userService,
            CancellationToken token)
        {
            var user = await userService.GetCurrentUser();

            if (user == null)
            {
                return TypedResults.Unauthorized(); ;
            }

            //var existingUserLaptops = await userLaptopService.GetUserLaptops(command.UserID, context);

            //if (existingUserLaptops.Any())
            //{
            //    foreach (var userLaptop in existingUserLaptops)
            //    {
            //        userLaptop.Status = UserLaptopStatus.UnAssigned;
            //    }
            //}

            var requestBody = command.Body;

            var laptopToAdd = new Database.Entities.UserLaptop
            {
                //UserID = command.UserID,
                AssetName = requestBody.AssetName,
                Model = requestBody.Model,
                Comment = requestBody.Comment,
                AssetLocation = requestBody.AssetLocation,
                EmployeeDepartment = requestBody.EmployeeDepartment,
                Price = requestBody.Price,
                EstimationUsefulLifeYear = requestBody.EstimationUsefulLifeYear.ToUniversalTime(),
                DepreciationEstimationDate = requestBody.DepreciationEstimationDate.ToUniversalTime(),
                WarrantyExpirationDate = requestBody.WarrantyExpirationDate.ToUniversalTime(),
                PurchaseYear = requestBody.PurchaseYear.ToUniversalTime(),
                Created_At = DateTime.UtcNow.ToUniversalTime(),
                Modified = DateTime.UtcNow.ToUniversalTime()
            };


            context.UserLaptops.Add(laptopToAdd);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrail(user.Id, AuditTrailService.AuditAction.Create, AuditTrailService.AuditOn.Laptop, laptopToAdd.Id);

                    return TypedResults.Ok(new Response { LaptopId = laptopToAdd.Id });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest();


        }
    }
}
