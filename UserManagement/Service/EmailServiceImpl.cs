using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using UserManagement.Dto;
using UserManagement.Interface;
using UserManagement.CustomException;

namespace UserManagement.Service
{
    public class EmailServiceImpl : IEmail
    {
        private readonly EmailSettings _emailSettings;

        public EmailServiceImpl(IOptions<EmailSettings> settings)
        {
            this._emailSettings = settings.Value;
        }
        public  bool SendEmail(string to, string subject, string htmlMessage)
        {
            try
            {
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.FromEmail),
                        Subject = subject,
                        Body = htmlMessage,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(to);

                    client.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (SmtpException smtpEx)
            {
                throw new EmailSendingException("SMTP error occurred while sending email", smtpEx);
            }
            catch (InvalidOperationException invalidOpEx)
            {
                throw new EmailSendingException("Invalid operation occurred while sending email", invalidOpEx);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
