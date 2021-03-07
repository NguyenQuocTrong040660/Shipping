using Communication.Application.Common.Results;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Communication.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(MailMessage mailMessage);
        Task<Result> SendEmailWithReturnResult(MailMessage mailMessage);
        Task<string> GetEmailTemplate(string templateName);
        MailMessage BuildMailMessageForRegistration(string subject, string body, string emailTo, List<string> ccEmails, List<string> bccEmails);
    }
}
