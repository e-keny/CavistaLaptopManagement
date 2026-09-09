using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models
{
    public class LaptopHistory
    {
        public Guid Id { get; set; }

        public Guid UserLaptopID { get; set; }

        public Guid? ActionBy { get; set; }

        public string? ActionByName { get; set; }

        public string? Comment { get; set; }

        public UserLaptopHistoryStatus UserLaptopHistoryStatus { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
