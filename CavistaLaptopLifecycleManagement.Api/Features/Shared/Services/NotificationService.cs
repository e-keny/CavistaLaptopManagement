using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using CavistaLaptopLifecycleManagement.Api.Infrastructure.Emails;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CavistaLaptopLifecycleManagement.Api.Features.Shared.Services
{
    [RegisterScoped]
    public class NotificationService
    {
        private readonly CLMDbContext _context;
        private readonly MailService _mailService;

        public NotificationService(CLMDbContext context, MailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public async ValueTask NotifyUser(Guid userId, string message)
        {         
            var notificationToAdd = new Notification
            {
                UserId = userId,
                Message = message,
                IsRead = false,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            await _context.Notifications.AddAsync(notificationToAdd);

            var user = await _context.Users.Where(x => x.Id == userId && !x.IsDeprecated && !x.IsActive).FirstOrDefaultAsync();

            if (user != null)
            {
                var to = new List<string>() { user.EmailAddress };
                var emailMessage = new Message(to, $"Activity Notification", $"{message}");

                try
                {
                    await _mailService.SendEmailAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred => {ex.Message}");
                }
                
            }

            _context.SaveChanges();
        }

        public async ValueTask NotifyAttendant(Guid attendantId, string message)
        {
            var notificationToAdd = new Notification
            {
                UserId = attendantId,
                Message = message,
                IsRead = false,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            await _context.Notifications.AddAsync(notificationToAdd);

            var user = await _context.Users.Where(x => x.Id == attendantId && !x.IsDeprecated && x.IsActive).FirstOrDefaultAsync();

            if (user != null)
            {
                var to = new List<string>() { user.EmailAddress };
                var emailMessage = new Message(to, $"Activity Notification", $"{message}");

                try
                {
                    await _mailService.SendEmailAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred => {ex.Message}");
                }                
            }

            _context.SaveChanges();
        }

        public async ValueTask NotifyUserAndAttendant(Guid userId, string userMessage, Guid attendantId, string attendantMessage)
        {
            var userNotificationToAdd = new Notification
            {
                UserId = userId,
                Message = userMessage,
                IsRead = false,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            await _context.Notifications.AddAsync(userNotificationToAdd);

            var user = await _context.Users.Where(x => x.Id == userId && !x.IsDeprecated && x.IsActive).FirstOrDefaultAsync();

            if (user != null)
            {
                var to = new List<string>() { user.EmailAddress };
                var emailMessage = new Message(to, $"Activity Notification", $"{userMessage}");

                try
                {
                    await _mailService.SendEmailAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred => {ex.Message}");
                }
            }

            var attendantNotificationToAdd = new Notification
            {
                UserId = attendantId,
                Message = attendantMessage,
                IsRead = false,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            await _context.Notifications.AddAsync(attendantNotificationToAdd);

            var attendUser = await _context.Users.Where(x => x.Id == attendantId && !x.IsDeprecated && x.IsActive).FirstOrDefaultAsync();

            if (attendUser != null)
            {
                var attendanTo = new List<string>() { attendUser.EmailAddress };
                var attendantEmailMessage = new Message(attendanTo, $"Activity Notification", $"{attendantMessage}");
               

                try
                {
                    await _mailService.SendEmailAsync(attendantEmailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred => {ex.Message}");
                }
            }

            _context.SaveChanges();
        }

        public async ValueTask NotifyUserAndIT(Guid userId, string userMessage, string adminMessage)
        {
            var user = await _context.Users.Where(x => x.Id == userId && !x.IsDeprecated && x.IsActive).FirstOrDefaultAsync();

            var userNotificationToAdd = new Notification
            {
                UserId = user != null ? user.Id : userId,
                Message = userMessage,
                IsRead = false,
                Created_At = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
            };

            await _context.Notifications.AddAsync(userNotificationToAdd);

            if (user != null)
            {
                var to = new List<string>() { user.EmailAddress };
                var emailMessage = new Message(to, $"Activity Notification", $"{userMessage}");
                await _mailService.SendEmailAsync(emailMessage);
            }

            var adminList = await _context.Users.Where(x => x.Role == Role.Admin && !x.IsDeprecated).ToListAsync();

            foreach (var adminUser in adminList)
            {
                var attendantNotificationToAdd = new Notification
                {
                    UserId = adminUser.Id,
                    Message = adminMessage,
                    IsRead = false,
                    Created_At = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                };

                await _context.Notifications.AddAsync(attendantNotificationToAdd);
            }

            var adminEmailAddresses = adminList.Select(x => x.EmailAddress).ToList();
           
            if (adminEmailAddresses.Any())
            {
                var attendantEmailMessage = new Message(adminEmailAddresses, $"Activity Notification", $"{adminMessage}");
               
                try
                {
                    await _mailService.SendEmailAsync(attendantEmailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred => {ex.Message}");
                }
            }

            _context.SaveChanges();
        }

        public async ValueTask NotifyIT(string adminMessage)
        {
            var adminList = await _context.Users.Where(x => x.Role == Role.IT && !x.IsDeprecated).ToListAsync();

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

                await _context.Notifications.AddAsync(attendantNotificationToAdd);
            }

            var adminEmailAddresses = adminList.Select(x => x.EmailAddress).ToList();

            if (adminEmailAddresses.Any())
            {
                var attendantEmailMessage = new Message(adminEmailAddresses, $"Activity Notification", $"{adminMessage}");
               
                try
                {
                    await _mailService.SendEmailAsync(attendantEmailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred => {ex.Message}");
                }
            }

            _context.SaveChanges();
        }
    }
}
