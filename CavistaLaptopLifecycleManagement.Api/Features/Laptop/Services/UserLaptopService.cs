using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var userLaptops = await context.UserLaptops.Where(x => x.UserID == userId && !x.IsDeprecated && x.Condition == UserLaptopCondition.Active).ToListAsync();

            return userLaptops;
        }

        public async ValueTask<PaginatedList<Models.UserLaptop>> GetUserLaptops(int? pageNumber = 1, int? pageSize = 10)
        {
            var userLaptops = _context.UserLaptops
                .Where(x => !x.IsDeprecated && x.Condition == UserLaptopCondition.Active)
                .Select(x => new Models.UserLaptop
                {
                    UserID = x.Id,
                    AssetName = x.AssetName,
                    Model = x.Model,
                    Comment = x.Comment,
                    AssetLocation = x.AssetLocation,
                    EmployeeDepartment = x.EmployeeDepartment,
                    Condition = x.Condition,
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
