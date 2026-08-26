using System.ComponentModel.DataAnnotations.Schema;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class AuditTrail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Action { get; set; }

        public Guid ActionBy { get; set; }

        public string ActionOn { get; set; }

        public Guid ActionOnId { get; set; }

        public DateTimeOffset ActionAt { get; set; }
    }
}
