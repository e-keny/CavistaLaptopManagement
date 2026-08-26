using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using Immediate.Injections.Shared;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services
{
    [RegisterScoped<UserService>]
    public class UserService
    {
        public async ValueTask<IEnumerable<User>> GetUsers()
        {
            return new List<User>();
        }
    }
}
