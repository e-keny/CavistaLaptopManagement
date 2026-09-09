using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class TicketCommentDetail
    {
        public Guid UserLaptopID { get; set; }

        public Guid Id { get; set; }

        public Guid? OwnerId { get; set; }

        public string? OwnerName { get; set; }

        public string? Comment { get; set; }

        public string? AssignedTo { get; set; }

        public TicketHistoryStatus? TicketStatus { get; set; }

        public List<TicketComment> Comments { get; set; }
    }
}
