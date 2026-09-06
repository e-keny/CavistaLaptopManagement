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
using System.ComponentModel.DataAnnotations;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Endpoints
{
    [Handler]
    [MapPost("create")]
    [MapGroup<LaptopMapGroup>]
    public static partial class CreateUserLaptop
    {
        public sealed record CreateLaptopBody
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Asset name is required")]
            public required string AssetName { get; init; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Model is required")]
            public required string Model { get; init; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Comment is required")]
            public required string Comment { get; init; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Asset location is required")]
            public required string AssetLocation { get; init; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Employee department is required")]
            public required string EmployeeDepartment { get; init; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Price is required")]
            public required decimal Price { get; init; }

            public required DateTimeOffset EstimationUsefulLifeYear { get; init; }

            public required DateTimeOffset DepreciationEstimationDate { get; init; }

            public required DateTimeOffset WarrantyExpirationDate { get; init; }

            public required DateTimeOffset PurchaseYear { get; init; }
        }

        public sealed record Command
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

            var laptopToAdd = new Database.Entities.UserLaptop
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

            context.UserLaptops.Add(laptopToAdd);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrailAsync(user.Id, AuditTrailService.AuditAction.Create, AuditTrailService.AuditOn.Laptop, laptopToAdd.Id);

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
