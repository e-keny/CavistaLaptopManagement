using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints
{
    [Handler]
    [MapGet("")]
    [MapGroup<UserMapGroup>]
    [Authorize(Policy = Policies.ITRolePolicy)]
    public static partial class GetUsers
    {
        public record GetUsersQuery([FromQuery] int? pageNumber, [FromQuery] int? pageSize, [FromQuery] string? searchString);

        private async static ValueTask<Results<Ok<PaginatedList<User>>, BadRequest>> HandleAsync(
            GetUsersQuery request,
            UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
            bool searchStringIsNullOrEmpty = true;
            var searchString = string.Empty;

            if (!string.IsNullOrWhiteSpace(request.searchString))
            {
                searchString = request.searchString;
                searchStringIsNullOrEmpty = false;
            }

            var result = context.Users
               .Where(x => !x.IsDeprecated && (searchStringIsNullOrEmpty || x.FirstName.ToLower().Contains(searchString.ToLower()) || x.LastName.ToLower().Contains(searchString.ToLower())))
               .Include(x => x.Laptops.Where(x => !x.IsDeprecated))
           .Select(User.FromDatabaseEntity);

            var pagedResult = await PaginatedList<User>.CreateAsync(result, request?.pageNumber ?? 1, request?.pageSize ?? 10);

            var actionByIds = pagedResult.Item.SelectMany(x => x.UserLaptops, (user, userLaptop) => new { user, userLaptop })
                .SelectMany(userAndLaptop => userAndLaptop.userLaptop.LaptopHistories, (userAndLap, laptopHis) => laptopHis)
                .Select(x => x.ActionBy).ToList();

            var actionIdBySet = pagedResult.Item.SelectMany(x => x.UserLaptops, (user, userLaptop) => new { user, userLaptop })
                .SelectMany(userAndLaptop => userAndLaptop.userLaptop.LaptopHistories, (userAndLap, laptopHis) => laptopHis)
                .Select(x => x).ToList();

            var actionByLookup = context.Users.Where(x => actionByIds.Contains(x.Id)).ToLookup(x => x.Id);

            foreach (var res in actionIdBySet)
            {
                res.ActionByName = res.ActionBy.HasValue ? actionByLookup[res.ActionBy.Value].Select(x => x.FullName).FirstOrDefault() : default;
            }

            return TypedResults.Ok(pagedResult); 
        }
    }
}
