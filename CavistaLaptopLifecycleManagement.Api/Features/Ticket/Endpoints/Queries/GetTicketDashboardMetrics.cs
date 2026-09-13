using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries
{
    [Handler]
    [MapGet("dashboard-metric")]
    [MapGroup<TicketMapGroup>]
    public static partial class GetTicketDashboardMetrics
    {
        public record GetTicketDashboardQuery();

        private async static ValueTask<Results<Ok<TicketDashboardDetails>, BadRequest>> HandleAsync(
            GetTicketDashboardQuery _,
            CLMDbContext context,
            CancellationToken token)
        {
            var ticketList = await (from ticket in context.Tickets
                                 where!ticket.IsDeprecated
                                 select ticket).ToListAsync();

            var result = new TicketDashboardDetails
            {
                Total = ticketList.Count,
                Open = ticketList.Where(x => x.TicketStatus == TicketHistoryStatus.Open).Count(),
                Claimed = ticketList.Where(x => x.TicketStatus == TicketHistoryStatus.Claimed).Count(),
                Resolved = ticketList.Where(x => x.TicketStatus == TicketHistoryStatus.Resolved).Count(),
            };

            return TypedResults.Ok(result);
        }
    }
}