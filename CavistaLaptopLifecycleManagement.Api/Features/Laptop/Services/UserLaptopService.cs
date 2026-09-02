using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
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

        public async Task<IEnumerable<UserLaptop>> GetUserLaptops(Guid userId, CLMDbContext context)
        {
            var userLaptops = await context.UserLaptops.Where(x => x.UserId == userId && !x.IsDeprecated).ToListAsync();

            return userLaptops;
        }

        public async Task<UserLaptop?> GetUserLaptop(Guid laptopId, CLMDbContext context)
        {
            var userLaptops = await context.UserLaptops.Where(x => x.Id == laptopId && !x.IsDeprecated).FirstOrDefaultAsync();

            return userLaptops;
        }

        public async Task<User?> GetUser(Guid userId, CLMDbContext context)
        {
            var userLaptops = await context.Users.Where(x => x.Id == userId && !x.IsDeprecated).FirstOrDefaultAsync();

            return userLaptops;
        }

        public async Task<LaptopHistory?> GetLaptopLastStatus(Guid laptopId, CLMDbContext context)
        {
            var userLastLaptopHistory = await context.LaptopHistories.Where(x => x.UserLaptopID == laptopId && !x.IsDeprecated).OrderByDescending(X => X.Created_At).FirstOrDefaultAsync();

            return userLastLaptopHistory;
        }

        public async ValueTask<PaginatedList<Models.UserLaptop>> GetUserLaptops(int? pageNumber = 1, int? pageSize = 10)
        {
            var userLaptops = _context.UserLaptops                
                //.Where(x => !x.IsDeprecated && x.Status == UserLaptopStatus.Assigned)
                .Select(x => new Models.UserLaptop
                {
                    UserID = x.UserId,
                    AssetName = x.AssetName,
                    Model = x.Model,
                    Comment = x.Comment,
                    AssetLocation = x.AssetLocation,
                    EmployeeDepartment = x.EmployeeDepartment,
                    //Status = x.Status,
                    Price = x.Price,
                    EstimationUsefulLifeYear = x.EstimationUsefulLifeYear,
                    DepreciationEstimationDate = x.DepreciationEstimationDate,
                    WarrantyExpirationDate = x.WarrantyExpirationDate,
                    PurchaseYear = x.PurchaseYear                  
                });

            return await PaginatedList<Models.UserLaptop>.CreateAsync(userLaptops, pageNumber ?? 1, pageSize ?? 10);
        }
    }
}
