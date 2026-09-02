namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services
{
    public enum Role
    {
        General,
        None,
        IT,
        Admin,
    }

    public class Policies
    {
        public const string ITRolePolicy = "ITRolePolicy";
    }
}
