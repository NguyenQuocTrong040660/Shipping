using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class ShippingRequestController : BaseController
    {
        readonly ILogger<ShippingRequestController> _logger;
        public ShippingRequestController(IMediator mediator, ILogger<ShippingRequestController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
