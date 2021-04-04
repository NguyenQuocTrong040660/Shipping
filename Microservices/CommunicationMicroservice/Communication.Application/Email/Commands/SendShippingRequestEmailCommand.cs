using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.Application.Common.Results;
using Communication.Domain.Models;
using Communication.Application.Interfaces;
using Communication.Domain.Constants;
using System.Collections.Generic;

namespace Communication.Application.Email.Commands
{
    public class SendShippingRequestEmailCommand : IRequest<Result>
    {
        public ShippingRequestResponse ShippingRequest { get; set; }
    }

    public class SendShippingRequestEmailCommandHandler : IRequestHandler<SendShippingRequestEmailCommand, Result>
    {
        private readonly ILogger<SendShippingRequestEmailCommandHandler> _logger;
        private readonly IEmailService _emailService;

        public SendShippingRequestEmailCommandHandler(
            ILogger<SendShippingRequestEmailCommandHandler> logger,
            IEmailService emailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<Result> Handle(SendShippingRequestEmailCommand request, CancellationToken cancellationToken)
        {
            var rawTemplate = await _emailService.GetEmailTemplate(EmailTemplate.ShippingRequest);

            var body = BuildTemplate(rawTemplate, request.ShippingRequest);

            if (!string.IsNullOrWhiteSpace(request.ShippingRequest.LogisticDeptEmail))
            {
                var mailMessage = _emailService.BuildMailMessageForSending("Shipping Request Create Successfully", body, new List<string>
                {
                    request.ShippingRequest.LogisticDeptEmail
                }, null, null);

                _ = _emailService.SendEmail(mailMessage);
            }

            if (!string.IsNullOrWhiteSpace(request.ShippingRequest.ShippingDeptEmail))
            {
                var mailMessage = _emailService.BuildMailMessageForSending("Shipping Request Create Successfully", body, new List<string>
                {
                    request.ShippingRequest.ShippingDeptEmail
                }, null, null);

                _ = _emailService.SendEmail(mailMessage);
            }

            return Result.Success();
        }

        private static string BuildTemplate(string rawTemplate, ShippingRequestResponse shippingRequestResponse)
        {
            var shippingRequest = shippingRequestResponse.ShippingRequest;

            rawTemplate = rawTemplate.Replace("{{ shippingRequestId }}", shippingRequest.Identifier);
            rawTemplate = rawTemplate.Replace("{{ salesId }}", shippingRequest.SalesOrder);
            rawTemplate = rawTemplate.Replace("{{ purchaseOrder }}", shippingRequest.PurchaseOrder);
            rawTemplate = rawTemplate.Replace("{{ semlineNumber }}", shippingRequest.SalelineNumber);
            rawTemplate = rawTemplate.Replace("{{ customerName }}", shippingRequest.CustomerName);
            rawTemplate = rawTemplate.Replace("{{ shippingDate }}", shippingRequest.ShippingDate.ToString("dd/MM/yyyy"));
            rawTemplate = rawTemplate.Replace("{{ notes }}", shippingRequest.Notes);

            string shippingRequestStrings = "";

            string shippingRequestString = @"
                                     <tr>
                                        <td style='border: 1px solid #000000;'>{0}</td>
                                        <td style='border: 1px solid #000000;'>{1}</td>
                                        <td style='border: 1px solid #000000;'>{2}</td>
                                        <td style='border: 1px solid #000000;'>{3}</td>
                                        <td style='border: 1px solid #000000;'>{4}</td>
                                    </tr>";

            foreach (var item in shippingRequest.ShippingRequestDetails)
            {
                var itemString = string.Format(shippingRequestString, item.Product.ProductNumber, item.Quantity,
                    item.Price.ToString("C"), item.Amount.ToString("C"), item.ShippingMode);

                shippingRequestStrings = string.Concat(shippingRequestStrings, itemString);
            }

            rawTemplate = rawTemplate.Replace("{{ shippingRequestDetails }}", shippingRequestStrings);
            return rawTemplate;
        }
    }
}
