namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Models
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string RecipientEmail { get; set; }

        public string Message { get; set; }

        public bool Read { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
