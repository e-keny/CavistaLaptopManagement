using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.Json;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string? Auth0UserId { get; set; }

        public  string? EmailAddress { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public string? FullName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLogin { get; set; }

        public IReadOnlyList<int> Roles { get; set; } = [];

        public bool Equals(User? other) =>
            other != null
            && Id.Equals(other.Id);

        public static readonly Expression<Func<Database.Entities.User, User>> FromDatabaseEntity =
            u => new()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                MiddleName = u.MiddleName,
                FullName = u.FullName,
                Auth0UserId = u.Auth0UserId,
                EmailAddress = u.EmailAddress,
                IsActive = u.IsActive,
                LastLogin = u.LastLogin,
                Roles = ToRoles(u.Roles),
            };

        private static List<int> ToRoles(string roles)
        {
            var rolesList =  !string.IsNullOrWhiteSpace(roles) ? roles : JsonSerializer.Serialize(new List<string>());

            return !string.IsNullOrWhiteSpace(rolesList) ? JsonSerializer.Deserialize<List<int>>(rolesList)! : new List<int>();
        }            
    }

    [RouteGroup("api/users")]
    public sealed partial class UserMapGroup
    {
        private static void CustomizeGroup(RouteGroupBuilder group)
            => group
                //.RequireAuthorization(Policies.ITRolePolicy)
                .RequireAuthorization()
                .WithTags("Users");
    }
}
