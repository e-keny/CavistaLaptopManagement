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
            //        Role = Features.Users.Services.Role.General,
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
            //        Role = Features.Users.Services.Role.IT,
            //        Created_At = DateTime.Parse("2026-08-23").ToUniversalTime(),
            //        Modified = DateTime.Parse("2026-08-23").ToUniversalTime()
            //    },
            //};

            //foreach (User user in users)
            //{
            //    this.Users.Add(user);
            //}
            //this.SaveChanges();

            //var userLaptops = new Laptop[]
            //{
            //    new Laptop
            //    {
            //        UserId = users[0].Id,
            //        AssetName ="Hp-765",
            //        Model = "Hp3113",
            //        Comment = "Super fast",
            //        AssetLocation = "Lagos, Nigeria",
            //        EmployeeDepartment = "Engineering",
            //        LaptopStatus = LaptopHistoryStatus.Assigned,
            //        LaptopNumber = "CLM-rtyu89e90ww",
            //        Currency = "NGN",
            //        Receipt =  string.Empty,
            //        Price = 1000000,
            //        EstimationUsefulLifeYear = DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        DepreciationEstimationDate =DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        WarrantyExpirationDate = DateTime.UtcNow.AddYears(5).ToUniversalTime(),
            //        PurchaseYear = DateTime.UtcNow.ToUniversalTime(),
            //        Created_At = DateTime.UtcNow.ToUniversalTime(),
            //        Modified = DateTime.UtcNow.ToUniversalTime()
            //    },
            //    new Laptop
            //    {
            //        UserId = users[1].Id,
            //        AssetName ="Hp-990",
            //        Model = "Hp499",
            //        Comment = "Super fast",
            //        AssetLocation = "Lagos, Nigeria",
            //        EmployeeDepartment = "Engineering",
            //        LaptopStatus = LaptopHistoryStatus.Assigned,
            //        LaptopNumber = "CLM-rtyu893873",
            //        Currency = "NGN",
            //        Receipt =  string.Empty,
            //        Price = 1000000,
            //        EstimationUsefulLifeYear = DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        DepreciationEstimationDate =DateTime.UtcNow.AddYears(1).ToUniversalTime(),
            //        WarrantyExpirationDate = DateTime.UtcNow.AddYears(5).ToUniversalTime(),
            //        PurchaseYear = DateTime.UtcNow.ToUniversalTime(),
            //        Created_At = DateTime.UtcNow.ToUniversalTime(),
            //        Modified = DateTime.UtcNow.ToUniversalTime()
            //    }
            //};

            //foreach (Laptop userLaptop in userLaptops)
            //{
            //    this.Laptops.Add(userLaptop);
            //}
            //this.SaveChanges();
        }
    }
}
