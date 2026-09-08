using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models
{
    public class LaptopHistory
    {
        public Guid Id { get; set; }

        public Guid UserLaptopID { get; set; }

        public string? ActionBy { get; set; }

        public string? Comment { get; set; }

        public string? UserLaptopHistoryStatus { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
