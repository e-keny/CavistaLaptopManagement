namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class LaptopHistory : BaseEntity
    {
        public Guid UserLaptopID { get; set; }

        public Guid TicketID { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public Guid? ClosedBy { get; set; }

        public DateTimeOffset ClosedAt { get; set; }

        public LaptopHistoryStatus LaptopHistoryStatus { get; set; }

        public UserLaptop UserLaptop { get; set; }

        public Ticket Ticket { get; set; }
    }

    public enum LaptopHistoryStatus
    {
        None,
        Complaint,
        AttentionNeeded,
        NoIssue,
        Maintenance,
        Fixed
    }
}
