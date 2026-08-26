using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries
{
    [Handler]
    [MapGet("{ticketId}")]
    [MapGroup<TicketMapGroup>]
    public sealed partial class GetTicket
    {
        public record Query([FromRoute]Guid TicketId);

        private async static ValueTask<Results<Ok<Models.Ticket>, NotFound>> HandleAsync(
            Query request,
            //UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
            var ticket =  await context.Tickets.Where(x => x.Id == request.TicketId).Select(Models.Ticket.FromDatabaseEntity).FirstOrDefaultAsync(token);

            return ticket is not null
            ? TypedResults.Ok(ticket)
            : TypedResults.NotFound();
        }
    }
}
