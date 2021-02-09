using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class MovementRequestController : BaseController
    {
        readonly ILogger<MovementRequestController> _logger;
        public MovementRequestController(IMediator mediator, ILogger<MovementRequestController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
