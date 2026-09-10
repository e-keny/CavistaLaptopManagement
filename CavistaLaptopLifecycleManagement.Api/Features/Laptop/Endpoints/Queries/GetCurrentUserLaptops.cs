using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            var userLaptops = from userLaptop in context.Laptops
                              where !userLaptop.IsDeprecated
                              && userLaptop.UserId == currentUser.Id
                              join user in context.Users on userLaptop.UserId equals user.Id into users
                              from curUser in users.DefaultIfEmpty()
                              select new UserLaptop
                              {
                                  Id = userLaptop.Id,
                                  UserId = userLaptop.UserId,
                                  AssetName = userLaptop.AssetName,
                                  Model = userLaptop.Model,
                                  Comment = userLaptop.Comment,
                                  AssetLocation = userLaptop.AssetLocation,
                                  EmployeeDepartment = userLaptop.EmployeeDepartment,
                                  Price = userLaptop.Price,
                                  EstimationUsefulLifeYear = userLaptop.EstimationUsefulLifeYear,
                                  DepreciationEstimationDate = userLaptop.DepreciationEstimationDate,
                                  WarrantyExpirationDate = userLaptop.WarrantyExpirationDate,
                                  PurchaseYear = userLaptop.PurchaseYear,
                                  status = userLaptop.UserLaptopStatus,
                                  AssignedToEmail = curUser.EmailAddress,
                                  AssignedToName = curUser.FullName
                              };

            var pagedResult = await PaginatedList<UserLaptop>.CreateAsync(userLaptops, request.pageNumber ?? 1, request.pageSize ?? 10);

            var listOfLaptopIds = pagedResult.Item.Select(x => x.Id).ToList();

            var laptopHistoryList = await (from laptopHis in context.LaptopHistories
                                           where !laptopHis.IsDeprecated
                                           && listOfLaptopIds.Contains(laptopHis.UserLaptopID)
                                           join user in context.Users on laptopHis.ActionBy equals user.Id
                                           select new Models.LaptopHistory
                                           {
                                               Id = laptopHis.Id,
                                               UserLaptopID = laptopHis.UserLaptopID,
                                               ActionBy = laptopHis.ActionBy,
                                               ActionByName = user.FullName,
                                               Comment = laptopHis.Comment,
                                               UserLaptopHistoryStatus = laptopHis.UserLaptopHistoryStatus,
                                               CreatedAt = laptopHis.Created_At
                                           }).ToListAsync();

            var historyLookUp = laptopHistoryList.ToLookup(x => x.UserLaptopID);

            foreach (var result in pagedResult.Item)
            {
                result.LaptopHistories = historyLookUp[result.Id].OrderBy(x => x.CreatedAt).ToList();
            }

            return TypedResults.Ok(pagedResult);
        }
    }
}
