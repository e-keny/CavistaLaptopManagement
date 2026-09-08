using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Immediate.Validations.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;
using static CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Commands.AddComment;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Commands
{
    [Handler]
    [MapPost("create")]
    [MapGroup<TicketMapGroup>]
    public static partial class CreateTicket
    {
        [Validate]
        public sealed partial record CreateTicketBody : IValidationTarget<CreateTicketBody>
        {
            [NotEmpty]
            public required string Description { get; init; }

            [NotEmpty]
            public required string Comment { get; init; }
        }

        [Validate]
        public sealed partial record Command : IValidationTarget<Command>
        {
            [FromBody]
            public required CreateTicketBody Body { get; init; }
        }

        public sealed record CreateTicketResponse
        {
            public Guid? TicketId { get; init; }

            public string? Message { get; init; }

            public CreateTicketResponse(string message)
            {
                Message = message;
                TicketId = null;
            }

            public CreateTicketResponse(Guid id)
            {
                Message = "successful";
                TicketId = id;
            }
        }


        private async static ValueTask<Results<Ok<CreateTicketResponse>, BadRequest<CreateTicketResponse>, NotFound<CreateTicketResponse>, UnauthorizedHttpResult>> HandleAsync(
            Command request,
            UserService userService,
            AuditTrailService auditTrailService,
            NotificationService notificationService,
            CLMDbContext context,
            CancellationToken token)
        {
            var currentUser = await userService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            var ticketToAdd = new Database.Entities.Ticket
            {
                Description = request.Body.Description,
                Comment = request.Body.Comment,
                UserId = currentUser.Id, 
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            context.Tickets.Add(ticketToAdd);

            var userLaptop = await context.UserLaptops
                .Where(x => x.UserId == currentUser.Id && !x.IsDeprecated)
                .FirstOrDefaultAsync(token);

            if (userLaptop == null)
            {
                return TypedResults.NotFound(new CreateTicketResponse("Laptop not found for this user"));
            }

            var historyToAdd = new Database.Entities.TicketHistory
            {
                UserLaptopID = userLaptop.Id,
                TicketID = ticketToAdd.Id,
                TicketHistoryStatus = TicketHistoryStatus.Open,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            context.TicketHistories.Add(historyToAdd);

            await auditTrailService.AddAuditTrailAsync(context, currentUser.Id, AuditTrailService.AuditAction.Create, AuditTrailService.AuditOn.Ticket, ticketToAdd.Id);

            var notificationMessage = $"There is an available ticket waiting to be treated";

            await notificationService.NotifyIT(context, notificationMessage);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    return TypedResults.Ok(new CreateTicketResponse(ticketToAdd.Id));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest(new CreateTicketResponse("Failed to create ticket"));
        }
    }
}
