using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Commands
{
    [Handler]
    [MapPost("{ticketId}/add-comment")]
    [MapGroup<TicketMapGroup>]
    public static partial class AddComment
    {
        public sealed record AddTicketcommentBody
        {
            public required string Message { get; init; }
        }

        public sealed record Command
        {
            [FromRoute]
            public required Guid TicketId { get; init; }

            [FromBody]
            public required AddTicketcommentBody AddTicketcommentBody { get; init; }
        }

        public sealed record AddTicketCommentResponse
        {
            public Guid? CommentId { get; init; }

            public string? Message { get; init; }

            public AddTicketCommentResponse(string message)
            {
                Message = message;
                CommentId = null;
            }

            public AddTicketCommentResponse(Guid id)
            {
                Message = "successful";
                CommentId = id;
            }
        }

        private async static ValueTask<Results<Ok<AddTicketCommentResponse>, BadRequest<AddTicketCommentResponse>, NotFound<AddTicketCommentResponse>, UnauthorizedHttpResult>> HandleAsync(
            Command request,
            UserService userService,
            AuditTrailService auditTrailService,
            CLMDbContext context,
            CancellationToken token)
        {
            var currentUser = await userService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            var ticket = await context.Tickets
                    .Where(x => x.Id == request.TicketId && !x.IsDeprecated)
                    .FirstOrDefaultAsync(token);

            if (ticket == null)
            {
                return TypedResults.NotFound(new AddTicketCommentResponse("Ticket not found"));
            }

            var ticketCommentToAdd = new Database.Entities.TicketComment
            {
                TicketId = request.TicketId,
                AuthorId = currentUser.Id,
                Comment = request.AddTicketcommentBody.Message,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            context.TicketComments.Add(ticketCommentToAdd);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrailAsync(currentUser.Id, AuditTrailService.AuditAction.Create, AuditTrailService.AuditOn.TicketComment, ticketCommentToAdd.Id);

                    return TypedResults.Ok(new AddTicketCommentResponse(ticketCommentToAdd.Id));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest(new AddTicketCommentResponse("Failed to create ticket comment"));
        }
    }
}
