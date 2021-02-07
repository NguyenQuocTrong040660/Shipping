using Files.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Files.Api.Controllers
{
    public class ExportController : BaseController
    {
        private readonly IDataService _dataService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ExportController> _logger;

        public ExportController(IMediator mediator,
            IWebHostEnvironment environment,
            ILogger<ExportController> logger,
            IDataService dataService) : base(mediator)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok();
        }
    }
}
