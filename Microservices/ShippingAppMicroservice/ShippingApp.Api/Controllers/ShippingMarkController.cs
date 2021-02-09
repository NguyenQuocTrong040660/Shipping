using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class ShippingMarkController : BaseController
    {
        readonly ILogger<ShippingMarkController> _logger;
        public ShippingMarkController(IMediator mediator, ILogger<ShippingMarkController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
