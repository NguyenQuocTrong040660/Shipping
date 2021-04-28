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
    public class SendCustomerCommentEmailCommand : IRequest<Result>
    {
        public CustomerItemModel customerItemModel { get; set; }
    }

    public class SendCustomerCommentEmailCommandHandler : IRequestHandler<SendCustomerCommentEmailCommand, Result>
    {
        private readonly ILogger<SendCustomerCommentEmailCommandHandler> _logger;
        private readonly IEmailService _emailService;

        public SendCustomerCommentEmailCommandHandler(
            ILogger<SendCustomerCommentEmailCommandHandler> logger,
            IEmailService emailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<Result> Handle(SendCustomerCommentEmailCommand request, CancellationToken cancellationToken)
        {
            var rawTemplate = await _emailService.GetEmailTemplate(EmailTemplate.CustomerComment);

            var body = BuildTemplate(rawTemplate, request.customerItemModel);
            var mailMessage = _emailService.BuildMailMessageForSending("Lời nhắn từ khách hàng", body, new List<string> {"tamcammedia@gmail.com"}, null, null);
            _ = _emailService.SendEmail(mailMessage);
          

            return Result.Success();
        }

        private static string BuildTemplate(string rawTemplate, CustomerItemModel customerItemModel)
        {
            rawTemplate = rawTemplate.Replace("{{customerName}}", customerItemModel.CustomerName);
            rawTemplate = rawTemplate.Replace("{{homeAddress}}", customerItemModel.HomeAddress);
            rawTemplate = rawTemplate.Replace("{{phoneNumber}}", customerItemModel.PhoneNumber);
            rawTemplate = rawTemplate.Replace("{{emailAddress}}", customerItemModel.EmailAddress);
            rawTemplate = rawTemplate.Replace("{{remark}}", customerItemModel.Remark);

            return rawTemplate;
        }
    }
}
