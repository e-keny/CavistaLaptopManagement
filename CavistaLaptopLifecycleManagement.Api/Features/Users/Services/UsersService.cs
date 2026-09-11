using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services
{
    [RegisterScoped]
    public class UserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CLMDbContext cLMDbContext;
        private readonly AppSettings _appSettings;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            CLMDbContext cLMDbContext,
             IOptions<AppSettings> options
            //UserRolesCache userRolesCache
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cLMDbContext = cLMDbContext;
            _appSettings = options.Value;
        }

        public async ValueTask<Models.User?> GetCurrentUserAsync()
        {
            if (httpContextAccessor.HttpContext is { User: { } user })
            {
                var subjectId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (subjectId != null)
                {
                    var currentUser = cLMDbContext.Users.
                        Where(x => x.Auth0UserId == subjectId && !x.IsDeprecated)
                        .Select(Models.User.FromDatabaseEntity).FirstOrDefault();

                    return currentUser;
                }

                if (_appSettings.IsSwaggerCall)
                {
                    var currentUser = cLMDbContext.Users.
                             Where(x => x.Id == _appSettings.CurrentUserId && !x.IsDeprecated)
                             .Select(Models.User.FromDatabaseEntity).FirstOrDefault();

                    return currentUser;
                }
            }

            return default;
        }

        public bool IsAuthorized(Models.User user, params int[] allowedRoles)
        {
            return allowedRoles.Any(x => user.Role.Equals(x));
        }

        public async Task<Database.Entities.User?> GetUserAsync(Guid userId, CLMDbContext context)
        {
            var user = await context.Users.Where(x => x.Id == userId && !x.IsDeprecated).FirstOrDefaultAsync();

            return user;
        }        

        [StackTraceHidden]
            [DoesNotReturn]
            private static void ThrowInvalidUserId(string userId) =>
                throw new InvalidOperationException($"Unknown user id: {userId}");
        }
}
