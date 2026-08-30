using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services.Requirements;
using Immediate.Injections.Shared;
using Microsoft.AspNetCore.Authorization;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services.ClaimHandlers
{
    [RegisterScoped<IAuthorizationHandler>]
    public class ITRoleHandler : AuthorizationHandler<ITRoleRequirement>
    {

        private readonly CLMDbContext _db;

        public ITRoleHandler(CLMDbContext db)
        {
            _db = db;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,   ITRoleRequirement requirement)
        {
            var claimValue = context.User.FindFirst(requirement.ClaimType)?.Value;

            if (claimValue == null)
                return Task.CompletedTask;

            var user = _db.Users.
                        Where(x => x.Auth0UserId == claimValue && !x.IsDeprecated)
                        .Select(Models.User.FromDatabaseEntity).FirstOrDefault(); ;

            if (user != null && user.Roles.Where(x => x.Equals(Role.Admin.ToString())).Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
