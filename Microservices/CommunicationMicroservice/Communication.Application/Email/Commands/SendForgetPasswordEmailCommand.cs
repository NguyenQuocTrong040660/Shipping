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
    public class SendForgetPasswordEmailCommand : IRequest<Result>
    {
    }

    public class SendForgetPasswordEmailCommandHandler : IRequestHandler<SendForgetPasswordEmailCommand, Result>
    {
        private readonly ILogger<SendCompletedEventHandler> _logger;
        private readonly IEnvironmentApplication _environment;

        public SendForgetPasswordEmailCommandHandler(
            IEnvironmentApplication environment,
            ILogger<SendCompletedEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task<Result> Handle(SendForgetPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            return Result.Success();
        }
    }
}
