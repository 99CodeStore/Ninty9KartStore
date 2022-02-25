using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace NintyNineKartStore.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly MailClientConfigurationOption mailClientConfigurationOption;
 
        public EmailSender()
        {
            this.mailClientConfigurationOption = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                FromEmail = "bheemstn@gmail.com",
                Password = "Ram+2016",
                UseTls = true,
                UseSsl = true,
            };
        }
        public EmailSender(IOptions<MailClientConfigurationOption> mailClientConfigurationOption)
        {
            this.mailClientConfigurationOption = mailClientConfigurationOption.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(mailClientConfigurationOption.FromEmail));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using (var emailClient = new SmtpClient())
            {

                await emailClient.ConnectAsync(mailClientConfigurationOption.Host,
                     mailClientConfigurationOption.Port, MailKit.Security.SecureSocketOptions.StartTls);

                await emailClient.AuthenticateAsync(
                     mailClientConfigurationOption.FromEmail,
                     mailClientConfigurationOption.Password
                     );

                await emailClient.SendAsync(emailToSend);

                await emailClient.DisconnectAsync(true);

            }
        }
    }
}
