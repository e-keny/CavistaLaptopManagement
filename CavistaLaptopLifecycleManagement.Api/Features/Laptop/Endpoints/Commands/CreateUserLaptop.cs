using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Immediate.Validations.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Endpoints
{
    [Handler]
    [MapPost("create")]
    [MapGroup<LaptopMapGroup>]
    [Authorize(Policy = Policies.ITRolePolicy)]
    public static partial class CreateUserLaptop
    {
        [Validate]
        public sealed partial record CreateLaptopBody : IValidationTarget<CreateLaptopBody>
        {
            [NotEmpty]
            public required string AssetName { get; init; }

            [NotEmpty]
            public required string Model { get; init; }

            [NotEmpty]
            public required string Comment { get; init; }

            [NotEmpty]
            public required string AssetLocation { get; init; }

            [NotEmpty]
            public required string EmployeeDepartment { get; init; }

            [NotEmpty]
            public required decimal Price { get; init; }

            public  DateTimeOffset EstimationUsefulLifeYear { get; init; }

            public  DateTimeOffset DepreciationEstimationDate { get; init; }

            public  DateTimeOffset WarrantyExpirationDate { get; init; }

            public  DateTimeOffset PurchaseYear { get; init; }
        }

        [Validate]
        public sealed partial record Command : IValidationTarget<Command>
        {
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
            var user = await userService.GetCurrentUserAsync();

            if (user == null)
            {
                return TypedResults.Unauthorized(); ;
            }

            var requestBody = command.Body;

            var laptopToAdd = new Database.Entities.Laptop
            {
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

            context.Laptops.Add(laptopToAdd);

            await auditTrailService.AddAuditTrailAsync(context, user.Id, AuditTrailService.AuditAction.Create, AuditTrailService.AuditOn.Laptop, laptopToAdd.Id);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
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
