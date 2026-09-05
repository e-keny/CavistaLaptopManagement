using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Extensions;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries
{
    [Handler]
    [MapGet("")]
    [MapGroup<TicketMapGroup>]
    public static partial class GetTickets
    {
        public record Query([FromQuery] int? pageNumber, [FromQuery] int? pageSize);

        public class TicketCommentDetail
        {
            public Guid UserLaptopID { get; set; }

            public Guid Id { get; set; }

            public string? Comment { get; set; }

            public string? AssignedTo { get; set; }

            public string? TicketStatus { get; set; }

            public List<TicketComment> Comments { get; set; }
        }

        private async static ValueTask<Results<Ok<PaginatedList<TicketCommentDetail>>, BadRequest>> HandleAsync(
            Query request,
            CLMDbContext context,
            CancellationToken token)
        {
            var userTicketList = from ticket in context.Tickets
                     where !ticket.IsDeprecated
                     join user in context.Users on ticket.UserId equals user.Id
                     where !user.IsDeprecated
                     join userLaptop in context.UserLaptops on user.Id equals userLaptop.UserId
                     select new TicketCommentDetail
                     {
                         UserLaptopID = userLaptop.Id,
                         Id = ticket.Id,
                         Comment = ticket.Comment,
                         AssignedTo = user.FirstName,
                         TicketStatus = ticket.TicketStatus.GetDescription()
                     };

            var ticketCommentList = await (from ticketComment in context.TicketComments
                                           where !ticketComment.IsDeprecated
                                           join user in context.Users on ticketComment.AuthorId equals user.Id
                                           where !user.IsDeprecated                                           
                                           select new TicketComment
                                           {
                                               Id = ticketComment.Id,
                                               TicketId = ticketComment.TicketId,
                                               AuthorName = user.FullName,
                                               AuthorEmail = user.EmailAddress,
                                               Message = ticketComment.Comment,                                               
                                               CreatedAt = ticketComment.Created_At
                                           }).ToListAsync();

            var commentLookUp = ticketCommentList.ToLookup(x => x.TicketId);

            var pagedResult = await PaginatedList<TicketCommentDetail>.CreateAsync(userTicketList, request.pageNumber ?? 1, request.pageSize ?? 10);

            foreach (var result in pagedResult.Item)
            {                         
                result.Comments = commentLookUp[result.Id].OrderBy(x => x.CreatedAt).ToList();
            }

            return TypedResults.Ok(pagedResult);
        }
    }
}
