using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Ticket.Services
{
    [RegisterScoped]
    public class TicketService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CLMDbContext cLMDbContext;

        public TicketService(
            IHttpContextAccessor httpContextAccessor,
            CLMDbContext cLMDbContext
            //UserRolesCache userRolesCache
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cLMDbContext = cLMDbContext;
        }

        public async Task<Database.Entities.Ticket?> GetTicketAsync(Guid ticketId, CLMDbContext context)
        {
            var ticket = await context.Tickets.Where(x => x.Id == ticketId && !x.IsDeprecated).FirstOrDefaultAsync();

            return ticket;
        }

        public async Task<TicketHistory?> GetTicketLastStatusAsync(Guid TicketId, CLMDbContext context)
        {
            var lastTicketHistory = await context.TicketHistories.Where(x => x.TicketID == TicketId && !x.IsDeprecated).OrderByDescending(X => X.Created_At).FirstOrDefaultAsync();

            return lastTicketHistory;
        }
    }
}
