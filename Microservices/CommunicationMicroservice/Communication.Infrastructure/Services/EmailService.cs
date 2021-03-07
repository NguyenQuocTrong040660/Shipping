using Communication.Application.Common.Results;
using Communication.Application.Interfaces;
using Communication.Domain.Configs;
using Communication.Domain.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Communication.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly AppSettings _appSettings;
        private readonly IEnvironmentApplication _environmentApplication;

        public EmailService(ILogger<EmailService> logger, IOptions<AppSettings> options, IEnvironmentApplication environmentApplication)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environmentApplication = environmentApplication ?? throw new ArgumentNullException(nameof(environmentApplication));
            _appSettings = options.Value;
        }

        public async Task SendEmail(MailMessage mailMessage)
        {
            try
            {
                var client = CreateSMTPClient(
                    _appSettings.Smtp.Username,
                    _appSettings.Smtp.Password, 
                    _appSettings.Smtp.Port,
                    _appSettings.Smtp.Host);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
        }
        public async Task<Result> SendEmailWithReturnResult(MailMessage mailMessage)
        {
            Result result = Result.Failure("Failed to send email");

            try
            {
                var client = CreateSMTPClient(
                    _appSettings.Smtp.Username,
                    _appSettings.Smtp.Password,
                    _appSettings.Smtp.Port,
                    _appSettings.Smtp.Host);

                await client.SendMailAsync(mailMessage);

                result = Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }

            return result;
        }
        public async Task<string> GetEmailTemplate(string templateName)
        {
            string templateEmail = string.Empty;
            using (StreamReader reader = new StreamReader(_environmentApplication.WebRootPath + ($"/EmailTemplates/{templateName}.html")))
            {
                templateEmail = await reader.ReadToEndAsync();
            }

            return templateEmail;
        }

        public MailMessage BuildMailMessageForRegistration(string subject, string body, string emailTo, List<string> ccEmails, List<string> bccEmails)
        {
            var message = new MailMessage();

            message.To.Add(new MailAddress(emailTo, emailTo));
            message.From = new MailAddress(_appSettings.Smtp.Username, _appSettings.Smtp.Username);

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            if (ccEmails != null)
            {
                foreach (var item in ccEmails)
                {
                    message.CC.Add(new MailAddress(item, item));
                }
            }
                
            if (bccEmails != null)
            {
                foreach (var item in bccEmails)
                {
                    message.Bcc.Add(new MailAddress(item, item));
                }

            }

            return message;
        }

        private static SmtpClient CreateSMTPClient(string username, string password, 
            int port = 587, 
            string host = "smtp.gmail.com",
            bool enableSsl = true)
        {
            return new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(username, password)
            };
        }
    }
}
