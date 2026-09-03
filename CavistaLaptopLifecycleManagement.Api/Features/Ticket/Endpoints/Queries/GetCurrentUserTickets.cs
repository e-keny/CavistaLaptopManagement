using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries
{
    [Handler]
    [MapGet("current-user")]
    [MapGroup<TicketMapGroup>]
    public static partial class GetCurrentUserTickets
    {
        public record Query([FromQuery] int? pageNumber, [FromQuery] int? pageSize);

        private async static ValueTask<Results<Ok<PaginatedList<Models.Ticket>>, UnauthorizedHttpResult>> HandleAsync(
            Query request,
            CLMDbContext context,
                       UserService userService,
            CancellationToken token)
        {
            var currentUser = await userService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            var ticket = context.Tickets.Where(t => t.UserId == currentUser.Id).Select(Models.Ticket.FromDatabaseEntity);

            return TypedResults.Ok(await PaginatedList<Models.Ticket>.CreateAsync(ticket, request.pageNumber ?? 1, request.pageSize ?? 10));
        }
    }
}
