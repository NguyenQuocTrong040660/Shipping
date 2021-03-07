using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Communication.Application.Common.Results;
using Communication.Application.Interfaces;
using Communication.Domain.Constants;
using System.Collections.Generic;
using Communication.Domain.Models;
using Communication.Application.Common.Extensions;

namespace Communication.Application.Email.Commands
{
    public class SendForgetPasswordEmailCommand : IRequest<Result>
    {
        public List<ResetPasswordResult> ResetPasswordResults { get; set; }
    }

    public class SendForgetPasswordEmailCommandHandler : IRequestHandler<SendForgetPasswordEmailCommand, Result>
    {
        private readonly ILogger<SendForgetPasswordEmailCommandHandler> _logger;
        private readonly IEmailService _emailService;

        public SendForgetPasswordEmailCommandHandler(
            ILogger<SendForgetPasswordEmailCommandHandler> logger,
            IEmailService emailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<Result> Handle(SendForgetPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var rawTemplate = await _emailService.GetEmailTemplate(EmailTemplate.ForgotPassword);

            foreach (var item in request.ResetPasswordResults)
            {
                if (string.IsNullOrWhiteSpace(item.Email) || string.IsNullOrWhiteSpace(item.Password) || string.IsNullOrWhiteSpace(item.UserName))
                {
                    _logger.LogError($"Failed when send email to user");
                    continue;
                }

                var body = BuildTemplate(rawTemplate, item.UserName, item.Password);
                var mailMessage = _emailService.BuildMailMessageForRegistration("Reset Password", body, item.Email, null, null);
                _ = _emailService.SendEmail(mailMessage);
            }

            return Result.Success();
        }

        private static string BuildTemplate(string rawTemplate, string userName, string passwordEncode)
        {
            rawTemplate = rawTemplate.Replace("{{userName}}", userName);
            rawTemplate = rawTemplate.Replace("{{newPassword}}", passwordEncode.ToBase64Decode());

            return rawTemplate;
        }
    }
}
