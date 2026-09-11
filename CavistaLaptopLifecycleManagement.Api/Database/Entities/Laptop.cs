using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class Laptop : BaseEntity
    {
        public Guid? UserId { get; set; }

        public string AssetName { get; set; }

        public string Model { get; set; }

        public string LaptopNumber { get; set; }

        public string Comment { get; set; }

        public string AssetLocation { get; set; }

        public string EmployeeDepartment { get; set; }

        public UserLaptopHistoryStatus UserLaptopStatus { get; set; }

        public Decimal Price { get; set; }

        public string Currency { get; set; }

        public string Receipt { get; set; }

        public DateTimeOffset? EstimationUsefulLifeYear { get; set; }

        public DateTimeOffset? DepreciationEstimationDate { get; set; }

        public DateTimeOffset? WarrantyExpirationDate { get; set; }

        public DateTimeOffset? PurchaseYear { get; set; }

        public User User { get; set; }

        public ICollection<LaptopHistory> LaptopHistories { get; set; }
    }
}