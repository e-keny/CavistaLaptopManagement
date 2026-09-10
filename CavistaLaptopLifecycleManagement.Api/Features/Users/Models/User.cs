using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Extensions;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
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

        public Role Role { get; set; }

        public IReadOnlyList<UserLaptop> UserLaptops { get; set; }

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
                Role = u.Role,
                UserLaptops = u.Laptops.Where(x => !x.IsDeprecated).Select(x => new UserLaptop 
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    AssetName = x.AssetName,
                    Model = x.Model,
                    Comment = x.Comment,
                    AssetLocation = x.AssetLocation,
                    EmployeeDepartment = x.EmployeeDepartment,
                    Price = x.Price,
                    EstimationUsefulLifeYear = x.EstimationUsefulLifeYear,
                    DepreciationEstimationDate = x.DepreciationEstimationDate,
                    WarrantyExpirationDate = x.WarrantyExpirationDate,
                    status = x.UserLaptopStatus,
                    AssignedToEmail = u.EmailAddress,
                    AssignedToName = u.FullName,
                    PurchaseYear = x.PurchaseYear,
                    LaptopHistories = x.LaptopHistories.Where(x => !x.IsDeprecated).Select(x => new LaptopHistory
                    {
                        Id = x.Id,
                        UserLaptopID = x.UserLaptopID,
                        Comment = x.Comment,
                        ActionBy = x.ActionBy,
                        UserLaptopHistoryStatus = x.UserLaptopHistoryStatus,
                        CreatedAt = x.Created_At
                    }).ToList(),
                }).ToList()
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
                .RequireAuthorization()
                .WithTags("Users");
    }
}