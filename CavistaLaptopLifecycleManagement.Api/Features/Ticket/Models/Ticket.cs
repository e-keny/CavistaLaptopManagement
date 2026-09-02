using Immediate.Apis.Shared;
using System.Linq.Expressions;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public List<TicketHistory>? TicketHistory { get; set; }

        public static readonly Expression<Func<Database.Entities.Ticket, Ticket>> FromDatabaseEntity =
        u => new()
        {
            Id = u.Id,
            UserId = u.UserId,
            Description = u.Description ?? string.Empty,
            Comment = u.Comment ?? string.Empty,
            TicketHistory = u.TicketHistories != null ? u.TicketHistories.Select(x => new TicketHistory
            {
                UserLaptopID = x.UserLaptopID,
                TicketID = x.TicketID,
               // LastModifiedBy = x.LastModifiedBy,
                Comment = x.Comment,
                ClosedAt = x.ClosedAt,
                //ActionBy = x.ActionBy,
                //AssignedTo = x.AssignedTo,
               // ResolvedBy = x.ResolvedBy,
               // TicketHistoryStatus = x.TicketHistoryStatus
            }).ToList() : new List<TicketHistory>()
        };
    }

    [RouteGroup("api/tickets")]
    public sealed partial class TicketMapGroup
    {
        private static void CustomizeGroup(RouteGroupBuilder group)
            => group
                .RequireAuthorization()
                .WithTags("Tickets");
    }
}
