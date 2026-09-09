using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class Ticket : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketNumber {  get; set; }

        public Guid UserId { get; set; }

        public Guid LaptopId { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Comment { get; set; }

        public TicketHistoryStatus? TicketStatus { get; set; }

        public ICollection<TicketHistory>? TicketHistories { get; set; }
    }
}
