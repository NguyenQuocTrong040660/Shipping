using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class ReceivedMarkController : BaseController
    {
        readonly ILogger<ReceivedMarkController> _logger;
        public ReceivedMarkController(IMediator mediator, ILogger<ReceivedMarkController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
