using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Extensions;
using CavistaLaptopLifecycleManagement.Api.Features.Ticket.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries.GetTickets;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Endpoints.Queries
{
    [Handler]
    [MapGet("current-user")]
    [MapGroup<TicketMapGroup>]
    public static partial class GetCurrentUserTickets
    {
        public record Query([FromQuery] int? pageNumber, [FromQuery] int? pageSize);

        private async static ValueTask<Results<Ok<PaginatedList<TicketCommentDetail>>, UnauthorizedHttpResult>> HandleAsync(
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


            var userTicketList = from ticket in context.Tickets
                                 where ticket.UserId == currentUser.Id
                                 && !ticket.IsDeprecated
                                 join user in context.Users on ticket.UserId equals user.Id
                                 where !user.IsDeprecated
                                 join userLaptop in context.UserLaptops on user.Id equals userLaptop.UserId into laptopList
                                 from laptop in laptopList.DefaultIfEmpty()
                                 join LaptopOwner in context.Users on ticket.UserId equals LaptopOwner.Id into laptopOwnerList
                                 from LaptopOwner in laptopOwnerList.DefaultIfEmpty()
                                 select new TicketCommentDetail
                                 {
                                     UserLaptopID = laptop.Id,
                                     Id = ticket.Id,
                                     Comment = ticket.Comment,
                                     AssignedTo = user.FirstName,
                                     OwnerId = LaptopOwner.Id,
                                     OwnerName = $"{LaptopOwner.FirstName}  {LaptopOwner.LastName}",
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
