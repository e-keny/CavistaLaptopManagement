namespace CavistaLaptopLifecycleManagement.Api.Features.Shared
{
    public class AppSettings
    {
        public const string AppSettingSection = "AppSettings";

        public bool IsSwaggerCall { get; set; }

        public Guid CurrentUserId { get; set; }

        public string IdentityAddress { get; set; }


        public EmailConfiguration EmailConfiguration { get; set; }
    }

    public class EmailConfiguration
    {
        public string From { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
