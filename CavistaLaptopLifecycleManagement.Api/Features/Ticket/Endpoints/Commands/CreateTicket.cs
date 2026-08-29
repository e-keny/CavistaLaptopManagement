using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Commands
{
    [Handler]
    [MapPost("create")]
    [MapGroup<TicketMapGroup>]
    public sealed partial class CreateTicket
    {
        internal static Created<Response> TransformResult(Response response) =>
        TypedResults.Created($"/api/tickets/{response.TicketId}", response);

        public sealed record CreateTicketBody
        {
            public required string Description { get; init; }

            public required string Comment { get; init; }
        }

        public sealed record Command
        {
            [FromBody]
            public required CreateTicketBody Body { get; init; }
        }

        public sealed record Response
        {
            public required Guid TicketId { get; init; }
        }

        private async static ValueTask<Results<Ok<Response>, BadRequest, NotFound>> HandleAsync(
            Command request,
            //UserLaptopService userLaptopService,
            AuditTrailService auditTrailService,
            CLMDbContext context,
            CancellationToken token)
        {
            var userId = Guid.Parse("01a03ff2-37c0-7ed8-8db2-de9a8b790fbf"); //Replace with logged in user

            var ticketToAdd = new Database.Entities.Ticket
            {
                Description = request.Body.Description,
                Comment = request.Body.Comment,
                UserId = userId, 
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            context.Tickets.Add(ticketToAdd);

            var userLaptop = await context.UserLaptops
                .Where(x => x.UserID == userId && x.Condition == UserLaptopCondition.Active && !x.IsDeprecated)
                .FirstOrDefaultAsync(token);

            if (userLaptop == null)
            {
                return TypedResults.NotFound();
            }

            var historyToAdd = new LaptopHistory
            {
                UserLaptopID = userLaptop.Id,
                TicketID = ticketToAdd.Id,
                LaptopHistoryStatus = LaptopHistoryStatus.None,
                Created_At = DateTime.UtcNow, 
                Modified = DateTime.UtcNow,
            };

            context.LaptopHistories.Add(historyToAdd);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrail(userId, AuditTrailService.AuditAction.Create, AuditTrailService.AuditOn.Ticket, ticketToAdd.Id);

                    return TypedResults.Ok(new Response { TicketId = ticketToAdd.Id });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest();
        }

    }
}
