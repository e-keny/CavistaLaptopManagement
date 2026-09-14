using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using CavistaLaptopLifecycleManagement.Api.Infrastructure.Emails;
using CavistaLaptopLifecycleManagement.Api.Infrastructure.Worker;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CavistaLaptopLifecycleManagement.Api.Features.Shared.Services
{
    [RegisterScoped]
    public class NotificationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MailService _mailService;

        private readonly IBackgroundTaskQueue _taskQueue;


        public NotificationService(IServiceProvider serviceProvider, MailService mailService, IBackgroundTaskQueue taskQueue)
        {
            _serviceProvider = serviceProvider;
            _mailService = mailService;
            _taskQueue = taskQueue;
        }

        public async ValueTask NotifyUser(Guid userId,string message)
        {
            try
            {
                _taskQueue.QueueBackgroundWorkItem(async token =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<CLMDbContext>();

                    var notificationToAdd = new Notification
                    {
                        UserId = userId,
                        Message = message,
                        IsRead = false,
                        Created_At = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                    };

                    await context.Notifications.AddAsync(notificationToAdd);

                    var user = await context.Users.Where(x => x.Id == userId && !x.IsDeprecated && x.IsActive).FirstOrDefaultAsync();

                    context.SaveChanges();

                    //if (user != null)
                    //{
                    //    var to = new List<string>() { user.EmailAddress };
                    //    var emailMessage = new Message(to, $"Activity Notification", $"{message}");

                    //    await _mailService.SendEmailAsync(emailMessage);
                    //}       
                });

            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }
        }

        public async ValueTask NotifyIT(string adminMessage)
        {
            try
            {               
                _taskQueue.QueueBackgroundWorkItem(async token =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<CLMDbContext>();

                    var adminList = await context.Users.Where(x => x.Role == Role.IT && !x.IsDeprecated).ToListAsync();

                    foreach (var user in adminList)
                    {
                        var attendantNotificationToAdd = new Notification
                        {
                            UserId = user.Id,
                            Message = adminMessage,
                            IsRead = false,
                            Created_At = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                        };

                        await context.Notifications.AddAsync(attendantNotificationToAdd);
                    }

                    var adminEmailAddresses = adminList.Select(x => x.EmailAddress).ToList();

                    //if (adminEmailAddresses.Any())
                    //{
                    //    var attendantEmailMessage = new Message(adminEmailAddresses, $"Activity Notification", $"{adminMessage}");

                    //    await _mailService.SendEmailAsync(attendantEmailMessage);
                    //}

                    context.SaveChanges();
                });
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }          
        }
    }
}
