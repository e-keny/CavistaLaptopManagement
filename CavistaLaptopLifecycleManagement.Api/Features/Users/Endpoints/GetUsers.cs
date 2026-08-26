using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints
{
    [Handler]
    [MapGet("")]
    [MapGroup<UserMapGroup>]
    public sealed partial class GetUsersQuery
    {
        public record Query;

        private async static ValueTask<IEnumerable<User>> HandleAsync(
            Query _,
            //UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
            return await context.Users
            .Select(User.FromDatabaseEntity)
            .ToListAsync(token);
            //return userService.GetUsers();
        }
    }
}
