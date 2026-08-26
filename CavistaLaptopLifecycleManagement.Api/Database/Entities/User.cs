using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using System.ComponentModel.DataAnnotations;

namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class User : BaseEntity
    {
        public string? Auth0UserId { get; set; }

        public required string EmailAddress { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [MaxLength(50)]
        public string? MiddleName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLogin { get; set; }

        public string Roles { get; set; } = string.Empty;

        public ICollection<UserLaptop> UserLaptops { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName + " " + MiddleName;
            }
        }
    }
}
