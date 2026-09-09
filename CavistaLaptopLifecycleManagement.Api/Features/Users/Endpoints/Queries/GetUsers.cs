using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
                .Where(x => !x.IsDeprecated)
                .Include(x => x.UserLaptops.Where(x => !x.IsDeprecated))
            .Select(User.FromDatabaseEntity)
            .ToListAsync(token);

            var actionByIds = result.SelectMany(x => x.UserLaptops, (user, userLaptop) => new { user, userLaptop })
                .SelectMany(userAndLaptop => userAndLaptop.userLaptop.LaptopHistories, (userAndLap, laptopHis) => laptopHis)
                .Select(x => x.ActionBy).ToList();

            var actionIdBySet = result.SelectMany(x => x.UserLaptops, (user, userLaptop) => new { user, userLaptop })
                .SelectMany(userAndLaptop => userAndLaptop.userLaptop.LaptopHistories, (userAndLap, laptopHis) => laptopHis)
                .Select(x => x).ToList();

            var actionByLookup = context.Users.Where(x => actionByIds.Contains(x.Id)).ToLookup(x => x.Id);

            foreach (var res in actionIdBySet)
            {
                res.ActionByName = res.ActionBy.HasValue ? actionByLookup[res.ActionBy.Value].Select(x => x.FullName).FirstOrDefault() : default;
            }

            return TypedResults.Ok(result); 
        }
    }
}
