using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace CoctailsGuideWebApplication
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Site administration", "b5798ca3c86649"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.mailtrap.io", 2525, false);
                client.Authenticate("b5798ca3c86649", "70e570439638fd");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
