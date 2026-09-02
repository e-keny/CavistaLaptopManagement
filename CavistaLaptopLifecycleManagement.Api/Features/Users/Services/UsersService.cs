using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Services
{
    [RegisterSingleton<UserService>]
    public class UserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CLMDbContext cLMDbContext;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            CLMDbContext cLMDbContext
            //UserRolesCache userRolesCache
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cLMDbContext = cLMDbContext;
        }

        public async ValueTask<Models.User?> GetCurrentUser()
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

                return default;
            }

            return default;
        }

        public bool IsAuthorized(Models.User user, params int[] allowedRoles)
        {
            return allowedRoles.Any(x => user.Role.Equals(x));
        }

        public async Task<Database.Entities.User?> GetUser(Guid userId, CLMDbContext context)
        {
            var user = await context.Users.Where(x => x.Id == userId && !x.IsDeprecated).FirstOrDefaultAsync();

            return user;
        }


        //public async ValueTask<bool> IsAuthorized(string policy)
        //{
        //    if (await GetCurrentUser() is not { } user)
        //        return false;

        //    user = new(
        //        user
        //            .Identities
        //            .Where(i => i.AuthenticationType is not "Roles-Cache")
        //            .Append(
        //                await GetRoleClaimsIdentity(user)
        //            )
        //    );

        //    var auth = await authorizationService.AuthorizeAsync(user, policy);
        //    return auth.Succeeded;
        //}

        //public async ValueTask<bool> IsAdmin()
        //{
        //    var userId = await GetCurrentUserId();
        //    var roles = await userRolesCache.GetValue(new() { UserId = userId, }, CancellationToken.None);
        //    return roles.Contains("Admin", StringComparer.OrdinalIgnoreCase);
        //}

        //public async ValueTask<bool> IsInRole(string role)
        //{
        //    var userId = await GetCurrentUserId();
        //    var roles = await userRolesCache.GetValue(new() { UserId = userId, }, CancellationToken.None);
        //    return roles.Contains("Admin", StringComparer.OrdinalIgnoreCase) || roles.Contains(role, StringComparer.OrdinalIgnoreCase);
        //}

        //public async Task<ClaimsIdentity> GetRoleClaimsIdentity(ClaimsPrincipal principal)
        //{
        //    var claim = principal.FindFirstValue(Claims.Id) ?? "";
        //    if (!UserId.TryParse(claim, provider: null, out var userId))
        //        return new([], authenticationType: "Roles-Cache");

        //    var roles = await userRolesCache.GetValue(new() { UserId = userId, }, CancellationToken.None);

        //    return new(
        //        principal.Claims
        //            .Where(c => !string.Equals(c.Type, ClaimTypes.Role, StringComparison.Ordinal))
        //            .Concat(
        //                roles
        //                    .Select(r => new Claim(ClaimTypes.Role, r))
        //            ),
        //        authenticationType: "Roles-Cache"
        //    );
        //}

        //public async ValueTask<UserId> GetCurrentUserId()
        //{
        //    var user = await GetCurrentUser();

        //    var claim = user?.FindFirstValue(Claims.Id) ?? "";
        //    if (!UserId.TryParse(claim, provider: null, out var userId))
        //        ThrowInvalidUserId(claim);

        //    return userId;
        //}

        [StackTraceHidden]
            [DoesNotReturn]
            private static void ThrowInvalidUserId(string userId) =>
                throw new InvalidOperationException($"Unknown user id: {userId}");
        }
}
