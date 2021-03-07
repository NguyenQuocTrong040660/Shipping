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

namespace Communication.Application.Email.Commands
{
    public class SendEmailForTestingCommand : IRequest<Result>
    {
        public string To { get; set; }
        public string Content { get; set; }
    }

    public class SendEmailForTestingCommandHandler : IRequestHandler<SendEmailForTestingCommand, Result>
    {
        private readonly ILogger<SendEmailForTestingCommandHandler> _logger;
        private readonly IEmailService _emailService;

        public SendEmailForTestingCommandHandler(
            ILogger<SendEmailForTestingCommandHandler> logger,
            IEmailService emailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<Result> Handle(SendEmailForTestingCommand request, CancellationToken cancellationToken)
        {
            var mailMessage = _emailService.BuildMailMessageForRegistration("Spartronics Email for testing",
                request.Content, 
                request.To, null, null);

            await _emailService.SendEmail(mailMessage);
            return Result.Success();
        }
    }
}
