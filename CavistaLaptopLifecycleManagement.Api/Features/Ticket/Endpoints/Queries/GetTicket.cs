using CavistaLaptopLifecycleManagement.Api.Database;
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
    [MapGet("{ticketId}")]
    [MapGroup<TicketMapGroup>]
    public static partial class GetTicket
    {
        public record Query([FromRoute]Guid TicketId);

        private async static ValueTask<Results<Ok<TicketCommentDetail>, NotFound>> HandleAsync(
            Query request,
            CLMDbContext context,
            CancellationToken token)
        {
            var userTicket = await (from ticket in context.Tickets
                                 where ticket.Id == request.TicketId
                                 && !ticket.IsDeprecated
                                 join userLaptop in context.UserLaptops on ticket.LaptopId equals userLaptop.Id into laptopList
                                 from laptop in laptopList.DefaultIfEmpty()
                                 join user in context.Users on ticket.UserId equals user.Id
                                 where !user.IsDeprecated                                
                                 join LaptopOwner in context.Users on ticket.UserId equals LaptopOwner.Id into laptopOwnerList
                                 from LaptopOwner in laptopOwnerList.DefaultIfEmpty()
                                 select new TicketCommentDetail
                                 {
                                     TicketNumber = $"CLM-{ticket.TicketNumber:D8}",
                                     UserLaptopID = laptop != null ? laptop.Id : null,
                                     Id = ticket.Id,
                                     Comment = ticket.Comment,
                                     AssignedTo = user.FirstName,
                                     AssignedEmail = user.EmailAddress,
                                     OwnerId = LaptopOwner != null ? LaptopOwner.Id : null,
                                     OwnerName = LaptopOwner != null ? $"{LaptopOwner.FirstName}  {LaptopOwner.LastName}" : string.Empty,
                                     TicketStatus = ticket.TicketStatus
                                 }).FirstOrDefaultAsync(token);

            if (userTicket != null)
            {
                var ticketCommentList = await (from ticketComment in context.TicketComments
                                               where ticketComment.TicketId == request.TicketId
                                                   && !ticketComment.IsDeprecated
                                               join user in context.Users on ticketComment.AuthorId equals user.Id
                                               where !user.IsDeprecated
                                               select new Models.TicketComment
                                               {
                                                   Id = ticketComment.Id,
                                                   TicketId = ticketComment.TicketId,
                                                   AuthorName = user.FullName,
                                                   AuthorEmail = user.EmailAddress,
                                                   Message = ticketComment.Comment,
                                                   CreatedAt = ticketComment.Created_At
                                               }).ToListAsync();

                userTicket.Comments = ticketCommentList;
            }

            return userTicket is not null
            ? TypedResults.Ok(userTicket)
            : TypedResults.NotFound();
        }
    }
}
