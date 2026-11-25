using MedoraClinic.Application.Interfaces.IServices;
using MedoraClinic.Infrastructure.Services.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading;

namespace MedoraClinic.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions options;
        public EmailService(IOptions<EmailOptions> options) => this.options = options.Value;


        public Task SendEmailVerificationAsync(string email, string token)
        {
            var link = BuildLink("verify-email", token);

            var body = $"""
            <h2>تأكيد البريد الإلكتروني</h2>
            <p>افتح الرابط التالي لتأكيد حسابك:</p>
            <a href="{HtmlEncoder.Default.Encode(link)}">تأكيد البريد</a>
            <p>الرابط صالح لمدة 24 ساعة ويُستخدم مرة واحدة.</p>
            """;

            return SendAsync(
                email,
                "تأكيد حساب Medora Clinic",
                body);
        }

        public Task SendPasswordResetAsync(string email, string token)
        {
            var link = BuildLink("reset-password", token);

            var body = $"""
            <h2>إعادة تعيين كلمة المرور</h2>
            <p>افتح الرابط التالي لتعيين كلمة مرور جديدة:</p>
            <a href="{HtmlEncoder.Default.Encode(link)}">إعادة تعيين كلمة المرور</a>
            <p>الرابط صالح لمدة 20 دقيقة ويُستخدم مرة واحدة.</p>
            """;

            return SendAsync(
                email,
                "إعادة تعيين كلمة مرور Medora Clinic",
                body);
        }




        private string BuildLink(string route, string token)
        {
            var encodedToken = WebUtility.UrlEncode(token);

            return $"{options.FrontendBaseUrl.TrimEnd('/')}"
                + $"/{route}?token={encodedToken}";
        }

        private async Task SendAsync(
            string to,
            string subject,
            string body)
        {
            using var message = new MailMessage(
                options.From,
                to,
                subject,
                body)
            {
                IsBodyHtml = true
            };

            using var client = new SmtpClient(options.Host, options.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    options.Username,
                    options.AppPassword)
            };

            await client.SendMailAsync(message);
        }
    }
}
