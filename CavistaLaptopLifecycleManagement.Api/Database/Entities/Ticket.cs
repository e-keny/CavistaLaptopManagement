using System.ComponentModel.DataAnnotations;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class Ticket : BaseEntity
    {
        public Guid UserId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string Comment { get; set; }

        public LaptopHistory LaptopHistory { get; set; }
    }
}
