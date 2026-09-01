using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints
{
    [Handler]
    [MapGet("")]
    [MapGroup<UserMapGroup>]
    public static partial class GetUsersQuery
    {
        public record Query;

        private async static ValueTask<Results<Ok<List<User>>, BadRequest>> HandleAsync(
            Query _,
            UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
             var result = await context.Users
                .Include(x => x.UserLaptops)
            .Select(User.FromDatabaseEntity)
            .ToListAsync(token);

            return TypedResults.Ok(result); 
        }
    }
}
