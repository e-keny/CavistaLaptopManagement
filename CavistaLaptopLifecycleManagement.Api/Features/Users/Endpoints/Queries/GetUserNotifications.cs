using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Queries
{
    [Handler]
    [MapGet("user-notification")]
    [MapGroup<UserMapGroup>]
    public static partial class GetUserNotifications
    {
        public record Query;

        private async static ValueTask<Results<Ok<List<Notification>>, UnauthorizedHttpResult>> HandleAsync(
            Query _,
            UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
            var currentUser = await userService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            var result = await (from notification in context.Notifications
                         where notification.UserId == currentUser.Id
                         && !notification.IsDeprecated
                         join user in context.Users on notification.UserId equals user.Id
                         select new Notification
                         {
                             Id = notification.Id,
                             RecipientEmail = user.EmailAddress,
                             Message = notification.Message,
                             Read = notification.IsRead,
                             CreatedAt = notification.Created_At
                         }).ToListAsync();

            return TypedResults.Ok(result);
        }
    }
}