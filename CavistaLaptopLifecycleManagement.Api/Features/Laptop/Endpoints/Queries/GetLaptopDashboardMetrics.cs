using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries.GetTicketDashboardMetrics;

namespace CavistaLaptopLifecycleManagement.Api.Features.Laptop.Endpoints.Queries
{
    [Handler]
    [MapGet("dashboard-metric")]
    [MapGroup<LaptopMapGroup>]
    public static partial class GetLaptopDashboardMetrics
    {
        public record GetLaptopDashboardQuery();

    private async static ValueTask<Results<Ok<LaptopDashboardDetails>, BadRequest>> HandleAsync(
    GetLaptopDashboardQuery _,
    CLMDbContext context,
    CancellationToken token)
        {
            var ticketList = await (from laptop in context.Laptops
                                    where !laptop.IsDeprecated
                                    select laptop).ToListAsync();

            var result = new LaptopDashboardDetails
            {
                Total = ticketList.Count,
                Available = ticketList.Where(x => x.LaptopStatus == LaptopHistoryStatus.Available).Count(),
                Assigned = ticketList.Where(x => x.LaptopStatus == LaptopHistoryStatus.Assigned).Count(),
                InRepair = ticketList.Where(x => x.LaptopStatus == LaptopHistoryStatus.InRepair).Count(),
            };

            return TypedResults.Ok(result);
        }
    }
}
