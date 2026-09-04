using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Queries
{
    [Handler]
    [MapGet("current-user")]
    [MapGroup<UserMapGroup>]
    public static partial class GetCurrentUser
    {
        public record Query;

        private async static ValueTask<Results<Ok<List<User>>, UnauthorizedHttpResult>> HandleAsync(
            Query _,
            UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
            var currentUser = await userService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            var result = await context.Users.Where(x => x.Id == currentUser.Id && !x.IsDeprecated)
               .Include(x => x.UserLaptops)
           .Select(User.FromDatabaseEntity)
           .ToListAsync(token);

            return TypedResults.Ok(result);
        }
    }
}
