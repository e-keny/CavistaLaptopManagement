using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Queries
{
    [Handler]
    [MapGet("notification/{notificationId}")]
    [MapGroup<UserMapGroup>]
    public static partial class MarkNotificationAsRead
    {
        public record Query([FromRoute]Guid notificationId);

        private async static ValueTask<Results<Ok, NotFound<string>, UnauthorizedHttpResult>> HandleAsync(
            Query request,
            UserService userService,
            CLMDbContext context,
            CancellationToken token)
        {
            var currentUser = await userService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                return TypedResults.Unauthorized();
            }

            var existingNotification = await (from notification in context.Notifications
                                where notification.UserId == currentUser.Id
                                && notification.Id == request.notificationId
                                && !notification.IsDeprecated
                                select notification
                                ).FirstOrDefaultAsync();

            if (existingNotification == null)
            {
                return TypedResults.NotFound("Notification not found");
            }

            existingNotification.IsRead = true;
            existingNotification.Modified = DateTime.UtcNow;

            context.SaveChanges();
            
            return TypedResults.Ok();
        }
    }
}
