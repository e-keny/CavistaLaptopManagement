namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class TicketComment
    {
        public Guid Id { get; set; }

        public Guid? TicketId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public string Message { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
