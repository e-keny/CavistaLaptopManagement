using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Extensions;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Services
{
    [RegisterScoped<UserLaptopService>]
    public class UserLaptopService
    {
        private readonly CLMDbContext _context;

        public UserLaptopService(CLMDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Database.Entities.Laptop?>> GetUserLaptopsAsync(Guid userId, CLMDbContext context)
        {
            var userLaptops = await context.Laptops.Where(x => x.UserId == userId && !x.IsDeprecated).ToListAsync();

            return userLaptops;
        }

        public async Task<Database.Entities.Laptop?> GetLaptopByUserIdAsync(Guid userId, CLMDbContext context)
        {
            var userLaptop = await context.Laptops.Where(x => x.UserId == userId && !x.IsDeprecated).FirstOrDefaultAsync();

            return userLaptop;
        }

        public async Task<Database.Entities.Laptop?> GetUserLaptopAsync(Guid laptopId, CLMDbContext context)
        {
            var userLaptops = await context.Laptops.Where(x => x.Id == laptopId && !x.IsDeprecated).FirstOrDefaultAsync();

            return userLaptops;
        }

        public async Task<User?> GetUserAsync(Guid userId, CLMDbContext context)
        {
            var user = await context.Users.Where(x => x.Id == userId && !x.IsDeprecated).FirstOrDefaultAsync();

            return user;
        }

        public async Task<Database.Entities.LaptopHistory?> GetLaptopLastStatusAsync(Guid laptopId, CLMDbContext context)
        {
            var userLastLaptopHistory = await context.LaptopHistories.Where(x => x.UserLaptopID == laptopId && !x.IsDeprecated).OrderByDescending(X => X.Created_At).FirstOrDefaultAsync();

            return userLastLaptopHistory;
        }

        public async ValueTask<PaginatedList<Models.UserLaptop>> GetUserLaptopsAsync(int? pageNumber = 1, int? pageSize = 10)
        {
            var userLaptops = from userLaptop in _context.Laptops                
                where !userLaptop.IsDeprecated
                join user in _context.Users on userLaptop.UserId equals user.Id into users
                from curUser in users.DefaultIfEmpty()
                select new Models.UserLaptop
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

            var pagedResult = await PaginatedList<Models.UserLaptop>.CreateAsync(userLaptops, pageNumber ?? 1, pageSize ?? 10);

            var listOfLaptopIds = pagedResult.Item.Select(x => x.Id).ToList();

            var laptopHistoryList = await (from laptopHis in _context.LaptopHistories
                                           where !laptopHis.IsDeprecated
                                           && listOfLaptopIds.Contains(laptopHis.UserLaptopID)
                                           join user in _context.Users on laptopHis.ActionBy equals user.Id
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

            return pagedResult;
        }
    }
}
