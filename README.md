# CavistaLaptopManagement

💻 Laptop Management Application
📖 Overview
The Laptop Management Application is designed to help organizations efficiently manage their fleet of laptops. It provides tools for tracking inventory, monitoring usage, and ensuring compliance with IT policies.

🤝 Contributors and Authors (Cavars)

- [@E-keny](https://www.github.com/E-keny)

## Acknowledgements

 - [Cavista Technology](https://www.cavistatech.com/)

✨ Features
Inventory Tracking: Maintain records of all laptops, including serial numbers, models, and assigned users.

User Assignment: Easily assign and reassign laptops to employees.

🛠️ Tech Stack
Backend: .NET Core

Database: PostgreSQL

Authentication: OAuth2 / Active Directory Integration

Deployment: Docker

🚀 Getting Started
Prerequisites
.NET 10 SDK

PostgreSQL database

Docker

Installation
bash
# Clone the repository
git clone  https://github.com/Cavista-Technologies/The-Cavars-Backend-.git

# Navigate into the project
cd The-Cavars-Backend

# Install dependencies
dotnet restore   # for .NET backend
Running the Application
bash
# Start backend
dotnet run --project CavistaLaptopLifecycleManagement.Api

📂 Project Structure
Code
VSA Architecture
CavistaLaptopLifecycleManagement/
├── CavistaLaptopLifecycleManagement.Api/        # API services
│   ├── Database/                    # API endpoints
        │── Entities/                     # Entities (User, Laptop, Ticket etc.)
│   ├── Features/                        # Business logic
│       ├── Laptop/                    
│   │   ├── Ticket/                 
│   │   └── User/             
│   ├── Infrastructure/              # Data access
│   ├── Migration/                 
│   ├── Program.cs         
|── TestCavistaIdentityServer/       # Identity Server

Supports role-based access control (RBAC)

📜 Sample appsettings.json
{
  "ConnectionStrings": {
    "DBConnection": "Host=dpg-da7lcefavr4c73bcp2v0-a.ohio-postgres.render.com;Port=5432;Database=cavistalaptoplifecyclemanagement_vtvh;Username=admin;Password=f1kGRM1NC1zw2HDWat2CXzPalUMRyRa5"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "MongoURL": ""
  },
  "AllowedHosts": "*",
  "AppSettings": {
    "IsSwaggerCall": true,
    "CurrentUserId": "01a059b0-5b75-7462-bd42-2f950cbd0e9f",
    "IdentityAddress": "https://cavistatestidentityserver.onrender.com",
    "EmailConfiguration": {
      "From": "xxxx432@gmail.com",
      "SmtpServer": "smtp.gmail.com",
      "Port": 465,
      "Username": "XXXXX@gmail.com",
      "Password": "uitq gnpt yrfi plen"
    }
  }
}


🚦 Limitations

❌ Maintenance Requirements

📌 Future Improvements

✅ Asset Server integration

✅ Scheduling Maintenance

🤝 Contributing
Contributions are welcome! Please fork the repo and submit a pull request.

📜 License
MIT License — free to use and modify.