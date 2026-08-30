using Microsoft.AspNetCore.Authorization;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services.Requirements
{
    public class ITRoleRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }

        public ITRoleRequirement(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
