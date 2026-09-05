using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class UserTicketDetail
    {
        public Guid UserLaptopID { get; set; }

        public Guid Id { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public DateTimeOffset ClosedAt { get; set; }

        public Guid? ActionBy { get; set; }

        public string? Comment { get; set; }

        public string? AssignedTo { get; set; }

        public string? TicketStatus { get; set; }

        public List<TicketComment> Comments { get; set; }
    }
}
