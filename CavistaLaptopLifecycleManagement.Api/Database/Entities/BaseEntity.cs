using System.ComponentModel.DataAnnotations.Schema;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } 

        public bool IsDeprecated { get; set; }

        public DateTimeOffset Created_At { get; set; }

        public DateTimeOffset Modified { get; set; }
    }
}
