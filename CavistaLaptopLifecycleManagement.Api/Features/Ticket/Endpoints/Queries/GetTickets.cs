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

        private async static ValueTask<Results<Ok<PaginatedList<Models.UserTicketDetail>>, BadRequest>> HandleAsync(
            Query request,
            CLMDbContext context,
            CancellationToken token)
        {
            var userTicketList = from ticket in context.Tickets
                     where !ticket.IsDeprecated
                     join user in context.Users on ticket.UserId equals user.Id
                     where !user.IsDeprecated
                     join userLaptop in context.UserLaptops on user.Id equals userLaptop.UserId
                     select new UserTicketDetail
                     {
                         UserLaptopID = userLaptop.Id,
                         TicketID = ticket.Id,
                         Comment = ticket.Comment,
                         AssignedTo = user.FirstName
                     };

            var ticketHistoryList = await (from ticketHis in context.TicketHistories
                                       where !ticketHis.IsDeprecated
                                       join userLaptop in context.UserLaptops on ticketHis.UserLaptopID equals userLaptop.Id
                                       where !userLaptop.IsDeprecated
                                       join user in context.Users on userLaptop.UserId equals user.Id
                                       where !user.IsDeprecated
                                       join actionByUser in context.Users on ticketHis.ActionBy equals actionByUser.Id into lastModifyUser
                                       from actionBy in lastModifyUser.DefaultIfEmpty()
                                       join assignedToUser in context.Users on ticketHis.AssignedTo equals assignedToUser.Id into assignedToUser
                                       from assignedTo in assignedToUser.DefaultIfEmpty()
                                       join resolvedByUser in context.Users on ticketHis.ResolvedBy equals resolvedByUser.Id into resolvedByUser
                                       from resolveBy in resolvedByUser.DefaultIfEmpty()
                                       select new TicketHistory
                                       {                         
                                           TicketID = ticketHis.TicketID,
                                           UserLaptopID = userLaptop.Id,
                                           ClosedAt = ticketHis.ClosedAt,
                                           ActionBy = actionBy.FullName,
                                           AssignedTo = assignedTo.FullName,
                                           ResolvedBy = resolveBy.FullName,
                                           Comment = ticketHis.Comment,
                                           TicketHistoryStatus = ticketHis.TicketHistoryStatus.HasValue ? ticketHis.TicketHistoryStatus.GetDescription() : null,
                                           Created_At = ticketHis.Created_At
                                       }).ToListAsync();

            var historyLookUp = ticketHistoryList.ToLookup(x => x.TicketID);

            var pagedResult = await PaginatedList<UserTicketDetail>.CreateAsync(userTicketList, request.pageNumber ?? 1, request.pageSize ?? 10);

            foreach (var result in pagedResult.Item)
            {
                var lastHistory = historyLookUp[result.TicketID].OrderByDescending(x => x.Created_At).FirstOrDefault();

                if (lastHistory != null)
                {
                    result.TicketStatus = lastHistory.TicketHistoryStatus;
                }
                
                result.TicketHistory = historyLookUp[result.TicketID].ToList();
            }

            return TypedResults.Ok(pagedResult);
        }
    }
}
