using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Communication.Application.Common.Results;
using Communication.Application.Interfaces;
using System.Collections.Generic;
using Communication.Domain.Constants;
using Communication.Domain.Models;
using Communication.Application.Common.Extensions;

namespace Communication.Application.Email.Commands
{
    public class SendRegistrationEmailCommand : IRequest<Result>
    {
        public List<CreateUserResult> Users { get; set; }
    }

    public class SendRegistrationEmailCommandHandler : IRequestHandler<SendRegistrationEmailCommand, Result>
    {
        private readonly ILogger<SendCompletedEventHandler> _logger;
        private readonly IEmailService _emailService;

        public SendRegistrationEmailCommandHandler(
            ILogger<SendCompletedEventHandler> logger,
            IEmailService emailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<Result> Handle(SendRegistrationEmailCommand request, CancellationToken cancellationToken)
        {
            var rawTemplate = await _emailService.GetEmailTemplate(EmailTemplate.Registration);

            foreach (var item in request.Users)
            {
                if (string.IsNullOrWhiteSpace(item.Email) || string.IsNullOrWhiteSpace(item.Password) || string.IsNullOrWhiteSpace(item.UserName))
                {
                    _logger.LogError($"Failed when send email to user");
                    continue;
                }

                var body = BuildTemplate(rawTemplate, item.UserName, item.Password);
                var mailMessage = _emailService.BuildMailMessageForRegistration("Spartronics Registration Successfully", body, item.Email, null, null);
                _ = _emailService.SendEmail(mailMessage);
            }

            return Result.Success();
        }

        private static string BuildTemplate(string rawTemplate, string userName, string password)
        {
            rawTemplate = rawTemplate.Replace("{{userName}}", userName);
            rawTemplate = rawTemplate.Replace("{{password}}", password.ToBase64Decode());
            return rawTemplate;
        }
    }
}
