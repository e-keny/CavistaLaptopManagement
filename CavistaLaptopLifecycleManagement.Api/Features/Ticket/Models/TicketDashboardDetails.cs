namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class TicketDashboardDetails
    {
        public int Total { get; set; }

        public int Open { get; set; }

        public int Claimed { get; set; }

        public int Resolved { get; set; }
    }
}
