using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using Immediate.Injections.Shared;

namespace CavistaLaptopLifecycleManagement.Api.Features.Shared.Services
{
    [RegisterScoped]
    public class AuditTrailService
    {
        public static class AuditAction
        {
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "Delete";
        }

        public static class AuditOn
        {
            public const string Ticket = "Ticket";
            public const string User = "User";
            public const string Laptop = "Laptop";
            public const string LaptopHistory = "Laptop History";
        }

        private readonly CLMDbContext _context;

        public AuditTrailService(CLMDbContext context)
        {
            _context = context;
        }

        public async ValueTask AddAuditTrail(Guid actionBy, string action, string actionOn, Guid actionOnId)
        {
            var auditToAdd = new AuditTrail
            {

                ActionBy = actionBy,
                Action = action,
                ActionOn = actionOn,
                ActionOnId = actionOnId,
                ActionAt = DateTime.UtcNow
            };

            _context.AuditTrails.Add(auditToAdd);

            await _context.SaveChangesAsync();
        }
    }
}