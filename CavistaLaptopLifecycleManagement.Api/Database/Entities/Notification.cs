using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }
    }
}
