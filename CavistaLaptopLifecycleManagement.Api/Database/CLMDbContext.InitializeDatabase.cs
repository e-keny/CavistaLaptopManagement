using CavistaLaptopLifecycleManagement.Api.Database.Entities;

namespace CavistaLaptopLifecycleManagement.Api.Database
{
    public partial class CLMDbContext
    {
        public void InitializeDatabase()
        {
            //this.Database.EnsureCreated();

            //// Look for any students.
            //if (this.Users.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //var users = new User[]
            //{
            //    new User
            //    {
            //        Auth0UserId = "1",
            //        FirstName = "Alice",
            //        LastName = "Smith",
            //        EmailAddress = "AliceSmith@example.com",
            //        MiddleName = "Md",
            //        IsActive = true,
            //        LastLogin = DateTime.Parse("2026-08-23").ToUniversalTime(),
            //        Roles = "",
            //        Created_At = DateTime.Parse("2026-08-23").ToUniversalTime(),
            //        Modified = DateTime.Parse("2026-08-23").ToUniversalTime(),
            //    },
            //    new User
            //    {
            //        Auth0UserId = "2",
            //        FirstName = "Bob",
            //        LastName = "Alexander",
            //        EmailAddress = "BobSmith@example.com",
            //        MiddleName = "KC",
            //        IsActive = true,
            //        LastLogin = DateTime.Parse("2026-08-23").ToUniversalTime(),
            //        Roles = "",
            //        Created_At = DateTime.Parse("2026-08-23").ToUniversalTime(),
            //        Modified = DateTime.Parse("2026-08-23").ToUniversalTime()
            //    },
            //};

            //foreach (User user in users)
            //{
            //    this.Users.Add(user);
            //}
            //this.SaveChanges();

            //var userLaptops = new UserLaptop[]
            //{
            //    new UserLaptop
            //    {
            //        UserID = users[0].Id,
            //        AssetName ="Chemistry",
            //        Model = "Hp3113",
            //        Comment = "Super fast",
            //        AssetLocation = "Lagos, Nigeria",
            //        EmployeeDepartment = "Engineering",
            //        Condition = UserLaptopCondition.Active,
            //        Price = 1000000,
            //        EstimationUsefulLifeYear = DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        DepreciationEstimationDate =DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        WarrantyExpirationDate = DateTime.UtcNow.AddYears(5).ToUniversalTime(),
            //        PurchaseYear = DateTime.UtcNow.ToUniversalTime(),
            //        Created_At = DateTime.UtcNow.ToUniversalTime(),
            //        Modified = DateTime.UtcNow.ToUniversalTime()
            //    },
            //    new UserLaptop
            //    {
            //        UserID = users[1].Id,
            //        AssetName ="Chemistry",
            //        Model = "Hp3113",
            //        Comment = "Super fast",
            //        AssetLocation = "Lagos, Nigeria",
            //        EmployeeDepartment = "Engineering",
            //        Condition = UserLaptopCondition.Active,
            //        Price = 1000000,
            //        EstimationUsefulLifeYear = DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        DepreciationEstimationDate =DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        WarrantyExpirationDate = DateTime.UtcNow.AddYears(5).ToUniversalTime(),
            //        PurchaseYear = DateTime.UtcNow.ToUniversalTime(),
            //        Created_At = DateTime.UtcNow.ToUniversalTime(),
            //        Modified = DateTime.UtcNow.ToUniversalTime()
            //    }
            //};

            //    foreach (UserLaptop userLaptop in userLaptops)
            //    {
            //        this.UserLaptops.Add(userLaptop);
            //    }
            //    this.SaveChanges();
            }
        }
}
