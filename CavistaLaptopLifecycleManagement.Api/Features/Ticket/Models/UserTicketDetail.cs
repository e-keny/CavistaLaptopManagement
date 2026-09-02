using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class UserTicketDetail
    {
        public Guid UserLaptopID { get; set; }

        public Guid? TicketID { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public DateTimeOffset ClosedAt { get; set; }

        public Guid? ActionBy { get; set; }

        public string? Comment { get; set; }

        public string? AssignedTo { get; set; }

        public Guid? ResolvedBy { get; set; }

        public TicketHistoryStatus? TicketHistoryStatus { get; set; }

        public List<TicketHistory> TicketHistory { get; set; }
    }
}
