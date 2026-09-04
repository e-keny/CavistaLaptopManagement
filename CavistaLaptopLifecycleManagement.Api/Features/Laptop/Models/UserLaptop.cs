using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using Immediate.Apis.Shared;
using System.Linq.Expressions;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models
{
    public class UserLaptop
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }

        public string AssetName { get; set; }

        public string Model { get; set; }

        public string Comment { get; set; }

        public string AssetLocation { get; set; }

        public string EmployeeDepartment { get; set; }

        public Decimal Price { get; set; }

        public DateTimeOffset? EstimationUsefulLifeYear { get; set; }

        public DateTimeOffset? DepreciationEstimationDate { get; set; }

        public DateTimeOffset? WarrantyExpirationDate { get; set; }

        public DateTimeOffset? PurchaseYear { get; set; }

        public static readonly Expression<Func<Database.Entities.UserLaptop, UserLaptop>> FromDatabaseEntity =
        u => new()
        {
            Id = u.Id,
            UserId = u.UserId,
            Comment = u.Comment ?? string.Empty,
            AssetName = u.AssetName ?? string.Empty,
            AssetLocation = u.AssetLocation ?? string.Empty,
            EmployeeDepartment = u.EmployeeDepartment,
            Price = 0,
            Model = u.Model,
            EstimationUsefulLifeYear = u.EstimationUsefulLifeYear,
            DepreciationEstimationDate = u.DepreciationEstimationDate,
            WarrantyExpirationDate = u.WarrantyExpirationDate,
            PurchaseYear = u.PurchaseYear
            //LaptopHistories = u.LaptopHistories != null ? u.LaptopHistories.Select(x => new LaptopHistory
            //{
            //    UserLaptopID = x.UserLaptopID,
            //    Comment = x.Comment,
            //    ActionBy = x.ActionBy

            //}).ToList() : new List<TicketHistory>()
        };
        }


    [RouteGroup("api/laptops")]
    public sealed partial class LaptopMapGroup
    {
        private static void CustomizeGroup(RouteGroupBuilder group)
            => group
                //.RequireAuthorization()
                .WithTags("Laptops");
    }
}
