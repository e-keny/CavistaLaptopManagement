namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class LaptopHistory : BaseEntity
    {
        public Guid UserLaptopID { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public Guid? ActionBy { get; set; }

        public string? Comment { get; set; }

        public UserLaptopHistoryStatus UserLaptopHistoryStatus { get; set; }

        public UserLaptop UserLaptop { get; set; }
    }

    public enum UserLaptopHistoryStatus
    {
        Available,
        Assigned,
        UnAssigned,
        InRepair,
        Retired
    }
}
