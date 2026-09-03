using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Commands
{
    [Handler]
    [MapPost("{ticketId}/claim-resolve")]
    [MapGroup<TicketMapGroup>]
    public static partial class UpdateTicket
    {

        public sealed record UpdateTicketBody
        {
            public required TicketHistoryStatus TicketHistoryStatus { get; init; }

            public string? Comment { get; init; }
        }

        public sealed record Command
        {
            [FromRoute]
            public required Guid TicketId { get; init; }

            [FromBody]
            public required UpdateTicketBody Body { get; init; }
        }

        public sealed record UpdateTicketResponse
        {
            public Guid? TicketId { get; init; }

            public string? Message { get; init; }

            public UpdateTicketResponse(string message)
            {
                Message = message;
                TicketId = null;
            }

            public UpdateTicketResponse(Guid id)
            {
                Message = "successful";
                TicketId = id;
            }
        }

        private async static  ValueTask<Results<Ok<UpdateTicketResponse>, BadRequest<UpdateTicketResponse>
            , NotFound<UpdateTicketResponse>, UnauthorizedHttpResult, NoContent>> HandleAsync
            (Command command,
            TicketService ticketService,
            UserService userService,
            UserLaptopService userLaptopService,
            AuditTrailService auditTrailService,
            CLMDbContext context,
            CancellationToken token)
        {
            var requestBody = command.Body;

            var CurrentUser = await userService.GetCurrentUserAsync();

            if (CurrentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            if (!Enum.IsDefined(typeof(TicketHistoryStatus), requestBody.TicketHistoryStatus))
            {
                return TypedResults.BadRequest(new UpdateTicketResponse("Status does not exist"));
            }

            var existingTicket = await ticketService.GetTicketAsync(command.TicketId, context);

            if (existingTicket == null)
            {
                return TypedResults.BadRequest(new UpdateTicketResponse("Ticket not found"));
            }

            var existingTicketStatus = await ticketService.GetTicketLastStatusAsync(command.TicketId, context);

            if ((existingTicket != null && existingTicketStatus == null) || (existingTicket != null && existingTicketStatus != null && (existingTicketStatus.TicketHistoryStatus != requestBody.TicketHistoryStatus)))
            {
                existingTicket.TicketStatus = requestBody.TicketHistoryStatus;
            }
            else
            {
                return TypedResults.NoContent();
            }

            var laptop = await userLaptopService.GetLaptopByUserIdAsync(existingTicket.UserId, context);

            var ticketHistoryToAdd = new Database.Entities.TicketHistory
            {
                ActionBy = CurrentUser.Id,
                UserLaptopID = laptop?.Id ?? Guid.Empty,
                TicketID = existingTicket.Id,
                Comment = requestBody.Comment,
                TicketHistoryStatus = requestBody.TicketHistoryStatus,      
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            if (requestBody.TicketHistoryStatus == TicketHistoryStatus.Resolved)
            {
                ticketHistoryToAdd.ClosedAt = DateTime.UtcNow;
                ticketHistoryToAdd.ResolvedBy = CurrentUser.Id;
            }
            else if (requestBody.TicketHistoryStatus == TicketHistoryStatus.Claimed)
            {
                ticketHistoryToAdd.AssignedTo = CurrentUser.Id;
            }

            context.TicketHistories.Add(ticketHistoryToAdd);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrailAsync(CurrentUser.Id, AuditTrailService.AuditAction.Update, AuditTrailService.AuditOn.Ticket, existingTicket.Id);

                    return TypedResults.Ok(new UpdateTicketResponse(existingTicket.Id));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest(new UpdateTicketResponse("An error occurred while trying to update a laptop"));
        }
    }
}
