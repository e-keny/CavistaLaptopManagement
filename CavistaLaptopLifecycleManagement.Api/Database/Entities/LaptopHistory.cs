using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class LaptopHistory : BaseEntity
    {
        public Guid UserLaptopID { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public Guid? ActionBy { get; set; }

        public string? Comment { get; set; }

        public LaptopHistoryStatus UserLaptopHistoryStatus { get; set; }

        public Laptop UserLaptop { get; set; }
    }

    public enum LaptopHistoryStatus
    {
        Available,
        Assigned,
        UnAssigned,
        InRepair,
        Retired
    }
}
