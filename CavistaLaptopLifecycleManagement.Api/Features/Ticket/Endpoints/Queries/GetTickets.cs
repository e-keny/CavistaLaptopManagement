using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries
{
    [Handler]
    [MapGet("")]
    [MapGroup<TicketMapGroup>]
    public sealed partial class GetTickets
    {
        public record Query([FromQuery] int? pageNumber, [FromQuery] int? pageSize);

        private async static ValueTask<Results<Ok<PaginatedList<Models.Ticket>>, BadRequest>> HandleAsync(
            Query request,
            //UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
            var ticket =  context.Tickets.Select(Models.Ticket.FromDatabaseEntity);

            return TypedResults.Ok(await PaginatedList<Models.Ticket>.CreateAsync(ticket, request.pageNumber ?? 1, request.pageSize ?? 10));
            //return userService.GetUsers();
        }
    }
}
