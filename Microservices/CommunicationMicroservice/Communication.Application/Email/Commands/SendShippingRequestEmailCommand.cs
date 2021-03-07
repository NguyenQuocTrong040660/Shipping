using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Communication.Application.Common.Results;

namespace Communication.Application.Email.Commands
{
    public class SendShippingRequestEmailCommand : IRequest<Result>
    {
    }

    public class SendShippingRequestEmailCommandHandler : IRequestHandler<SendShippingRequestEmailCommand, Result>
    {
        private readonly ILogger<SendCompletedEventHandler> _logger;

        public SendShippingRequestEmailCommandHandler(
            ILogger<SendCompletedEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> Handle(SendShippingRequestEmailCommand request, CancellationToken cancellationToken)
        {
            return Result.Success();
        }
    }
}
