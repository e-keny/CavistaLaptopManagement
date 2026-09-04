using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Endpoints.Queries
{
    [Handler]
    [MapGet("current-user")]
    [MapGroup<LaptopMapGroup>]
    public static partial class GetCurrentUserLaptops
    {
        public record Query([FromQuery] int? pageNumber, [FromQuery] int? pageSize);

        private async static ValueTask<Results<Ok<PaginatedList<UserLaptop>>, UnauthorizedHttpResult>> HandleAsync(
            Query request,
            CLMDbContext context,
            UserService userService,
            CancellationToken token)
        {
            var currentUser = await userService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            var ticket = context.UserLaptops.Where(t => t.UserId == currentUser.Id).Select(UserLaptop.FromDatabaseEntity);

            return TypedResults.Ok(await PaginatedList<UserLaptop>.CreateAsync(ticket, request.pageNumber ?? 1, request.pageSize ?? 10));
        }
    }
}
