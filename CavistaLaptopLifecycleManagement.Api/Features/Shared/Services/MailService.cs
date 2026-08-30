using Immediate.Injections.Shared;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CavistaLaptopLifecycleManagement.Api.Features.Shared.Services
{
    [RegisterScoped]
    public class MailService
    {
        //private readonly MailSettings _mailSettings;
        //private readonly EmailConfiguration _emailConfig;
        //public MailService(IOptions<MailSettings> options, EmailConfiguration emailConfig)
        //{
        //    _mailSettings = options.Value;
        //    _emailConfig = emailConfig;
        //}

        //public async Task SendEmailAsync(Message message)
        //{
        //    var emailMessage = CreateEmailMessage(message);
        //    await Send(emailMessage);
        //}

        //private MimeMessage CreateEmailMessage(Message message)
        //{
        //    var emailMessage = new MimeMessage();
        //    emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
        //    emailMessage.To.AddRange(message.To);
        //    emailMessage.Subject = message.Subject;
        //    var bodyBuilder = new BodyBuilder();
        //    bodyBuilder.TextBody = message.Content;
        //    if (!string.IsNullOrWhiteSpace(message.Content) &&
        //        (message.Content.Contains("<html", StringComparison.OrdinalIgnoreCase) ||
        //         message.Content.Contains("<body", StringComparison.OrdinalIgnoreCase) ||
        //         message.Content.Contains("</")))
        //    {
        //        bodyBuilder.HtmlBody = message.Content;
        //    }
        //    emailMessage.Body = bodyBuilder.ToMessageBody();
        //    return emailMessage;
        //}
        //private async Task Send(MimeMessage mailMessage)
        //{
        //    using (var client = new SmtpClient())
        //    {
        //        try
        //        {
        //            // config values like username, password and from should be moved to server enviroment varibale, but it kept for dev testing
        //            // and peer programming with team members
        //            client.ServerCertificateValidationCallback = (object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors) => true;
        //            client.CheckCertificateRevocation = false;
        //            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.SslOnConnect);
        //            client.AuthenticationMechanisms.Remove("XOAUTH2");
        //            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
        //            var x = await client.SendAsync(mailMessage);
        //            Console.WriteLine(x);
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error($"An error occured: {ex.Message}");
        //            throw;
        //        }
        //        finally
        //        {
        //            client.Disconnect(true);
        //            client.Dispose();
        //        }
        //    }
        //}
    }
}
