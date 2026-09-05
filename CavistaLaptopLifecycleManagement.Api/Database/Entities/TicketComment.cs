namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class TicketComment : BaseEntity
    {
        public Guid TicketId { get; set; }

        public Guid AuthorId { get; set; }

        public string Comment { get; set; }
    }
}
