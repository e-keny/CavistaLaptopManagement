using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class TicketHistory
    {
        public Guid UserLaptopID { get; set; }

        public Guid? TicketID { get; set; }

        public DateTimeOffset ClosedAt { get; set; }

        public string? ActionBy { get; set; }

        public string? Comment { get; set; }

        public string? AssignedTo { get; set; }

        public string? ResolvedBy { get; set; }

        public string? TicketHistoryStatus { get; set; }

        public Laptop.Models.UserLaptop UserLaptop { get; set; }

        public DateTimeOffset Created_At { get; set; }
    }
}
