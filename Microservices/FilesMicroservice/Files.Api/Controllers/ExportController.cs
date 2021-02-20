using Files.Application.Common.Interfaces;
using Files.Domain.Template;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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

        [HttpGet("Template/{type}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult<string> GetExportTemplateUrl(TemplateType type)
        {
            string domain = GetDomain();
            string templateUrl = string.Empty;

            switch (type)
            {
                case TemplateType.Product:
                    templateUrl = $"{domain}/templates/{nameof(ProductTemplate)}.xlsx";
                    break;
                case TemplateType.ShippingPlan:
                    break;
                case TemplateType.WorkOrder:
                    templateUrl = $"{domain}/templates/{nameof(WorkOrderTemplate)}.xlsx";
                    break;
                default:
                    break;
            }

            return Ok(templateUrl);
        }
    }
}
