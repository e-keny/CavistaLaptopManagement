namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models
{
    public class LaptopDashboardDetails
    {
        public int Total { get; set; }

        public int Available { get; set; }

        public int Assigned { get; set; }

        public int InRepair { get; set; }
    }
}