using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using Immediate.Apis.Shared;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models
{
    public class UserLaptop
    {
        public Guid UserID { get; set; }

        public string AssetName { get; set; }

        public string Model { get; set; }

        public string Comment { get; set; }

        public string AssetLocation { get; set; }

        public string EmployeeDepartment { get; set; }

        public UserLaptopCondition Condition { get; set; }

        public Decimal Price { get; set; }

        public DateTimeOffset? EstimationUsefulLifeYear { get; set; }

        public DateTimeOffset? DepreciationEstimationDate { get; set; }

        public DateTimeOffset? WarrantyExpirationDate { get; set; }

        public DateTimeOffset? PurchaseYear { get; set; }
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
