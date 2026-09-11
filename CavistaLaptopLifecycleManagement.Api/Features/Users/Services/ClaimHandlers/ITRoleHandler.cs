using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services.Requirements;
using Immediate.Injections.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services.ClaimHandlers
{
    [RegisterScoped<IAuthorizationHandler>]
    public class ITRoleHandler : AuthorizationHandler<ITRoleRequirement>
    {

        private readonly CLMDbContext _db;
        private readonly AppSettings _appSettings;

        public ITRoleHandler(CLMDbContext db, IOptions<AppSettings> options)
        {
            _db = db;
            _appSettings = options.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,   ITRoleRequirement requirement)
        {
            if (_appSettings.IsSwaggerCall)
            {
                context.Succeed(requirement);

                return Task.CompletedTask;
            }                

            var claimValue = context.User.FindFirst(requirement.ClaimType)?.Value;

            if (claimValue == null)
                return Task.CompletedTask;

            var user = _db.Users.
                        Where(x => x.Auth0UserId == claimValue && !x.IsDeprecated)
                        .Select(Models.User.FromDatabaseEntity).FirstOrDefault(); ;

            if (user != null && user.Role.Equals(Role.IT))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
