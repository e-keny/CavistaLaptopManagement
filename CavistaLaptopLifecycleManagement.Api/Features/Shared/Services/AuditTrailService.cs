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
            public const string TicketComment = "Laptop";
            public const string LaptopHistory = "Laptop History";
            public const string Assignability = "Assignability";
            public const string Rapair = "Rapair";
        }

        public async ValueTask AddAuditTrailAsync(CLMDbContext context, Guid actionBy, string action, string actionOn, Guid actionOnId)
        {
            var auditToAdd = new AuditTrail
            {

                ActionBy = actionBy,
                Action = action,
                ActionOn = actionOn,
                ActionOnId = actionOnId,
                ActionAt = DateTime.UtcNow
            };

            context.AuditTrails.Add(auditToAdd);
        }
    }
}