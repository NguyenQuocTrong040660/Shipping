using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Api.Controllers
{
    public class WorkOrderController : BaseController
    {
        readonly ILogger<WorkOrderController> _logger;
        public WorkOrderController(IMediator mediator, ILogger<WorkOrderController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
