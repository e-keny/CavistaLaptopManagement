namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services
{
    public enum Role
    {
        None,
        Admin,
        IT,
        General
    }

    public class Policies
    {
        public const string ITRolePolicy = "ITRolePolicy";
    }
}
