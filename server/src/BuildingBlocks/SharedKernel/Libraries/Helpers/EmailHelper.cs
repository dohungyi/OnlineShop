using System.Net;
using System.Net.Mail;
using System.Text;
using SharedKernel.Core;
using SharedKernel.Libraries.Helpers.Models;

namespace SharedKernel.Libraries;

public class EmailHelper
{
    public static void SendMail(EmailOptionRequest request)
    {
        SendMail(new List<EmailOptionRequest>() { request });
    }

    public static void SendMail(List<EmailOptionRequest> requests)
    {
        try
        {
            foreach (var request in requests)
            {
                var message = new MailMessage(request.Sender, request.To)
                {
                    From = new MailAddress(request.Sender, request.DisplayName),
                    Subject = request.Subject,
                    SubjectEncoding = Encoding.UTF8,
                    Body = request.Body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = request.IsBodyHTML,
                    Priority = request.Priority,
                };
                message.ReplyToList.Add(new MailAddress(request.Sender));
                SmtpClient.Send(message);
            }
        }
        catch (Exception ex)
        {
            // Logging.Error(ex);
            throw;
        }
    }

    private static SmtpClient SmtpClient => new SmtpClient()
    {
        Host = DefaultEmailConfig.SMTPServer,
        Port = DefaultEmailConfig.Port,
        UseDefaultCredentials = false,
        EnableSsl = true,
        Credentials = new NetworkCredential(DefaultEmailConfig.UserName, DefaultEmailConfig.Password),
    };
}