using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class TicketHistory
    {
        public Guid UserLaptopID { get; set; }

        public Guid? TicketID { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public DateTimeOffset ClosedAt { get; set; }

        public Guid? ActionBy { get; set; }

        public string? Comment { get; set; }

        public Guid? AssignedTo { get; set; }

        public Guid? ResolvedBy { get; set; }

        public TicketHistoryStatus? TicketHistoryStatus { get; set; }

        public Laptop.Models.UserLaptop UserLaptop { get; set; }

        public Ticket? Ticket { get; set; }
    }
}
