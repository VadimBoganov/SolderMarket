using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace solder.Controllers
{
    public class EmailController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SendMessage(string fromEmail, string subject, string message)
        {
            await SendMailAsync(fromEmail, subject, message);
            return RedirectToAction("Index", "Home");
        }

        private async Task SendMailAsync(string fromEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(fromEmail, "Client"));
            emailMessage.To.Add(new MailboxAddress("Администрация сайта", "vadimboganov@gmail.com"));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = message};

            using(var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("vadimboganov@gmail.com", "vErbatim12,");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }

    
}