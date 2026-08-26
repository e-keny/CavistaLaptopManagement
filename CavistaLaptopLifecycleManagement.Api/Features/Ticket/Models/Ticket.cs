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

        public static readonly Expression<Func<Database.Entities.Ticket, Ticket>> FromDatabaseEntity =
        u => new()
        {
            Id = u.Id,
            UserId = u.UserId,
            Description = u.Description,
            Comment = u.Comment,
        };
    }

    [RouteGroup("api/tickets")]
    public sealed partial class TicketMapGroup
    {
        private static void CustomizeGroup(RouteGroupBuilder group)
            => group
                //.RequireAuthorization()
                .WithTags("Tickets");
    }
}
