using Files.Application.Attachments.Commands;
using Files.Application.Common.Interfaces;
using Files.Domain.Enumerations;
using Files.Domain.Models;
using Files.Domain.Template;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Files.Api.Controllers
{
    public class ImportController : BaseController
    {
        private readonly IDataService _dataService;
        private readonly ILogger<ImportController> _logger;
        private readonly IUploadFileService _uploadService;

        public ImportController(IMediator mediator,
            IUploadFileService uploadService,
            ILogger<ImportController> logger,
            IDataService dataService) : base(mediator)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
        }

        [HttpPut("{type}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> ImportDataAsync([FromRoute] TemplateType type, IFormFile file)
        {
            (Result resultUpload, string fileUrl, string fileName) = _uploadService.UploadFile(file, GetDomain(), AttachmentTypes.Excel);

            dynamic data = null;

            if (resultUpload.Succeeded)
            {
                AttachmentDto attachment = InitAttachment(file, fileUrl, fileName);

                switch (type)
                {
                    case TemplateType.Product:
                        data = _dataService.ReadFromExcelFile<ProductTemplate>(fileUrl);
                        break;
                    case TemplateType.WorkOrder:
                        data = _dataService.ReadFromExcelFile<WorkOrderTemplate>(fileUrl);
                        break;
                    default:
                        break;
                }

                _logger.LogInformation("Upload result", resultUpload);
                await Mediator.Send(new AddAttachmentCommand() { Model = attachment, AttachmentType = AttachmentTypes.Excel });
            }    

            if (data == null)
            {
                return Ok(Result.Failure($"Failed to import data for {nameof(type)}"));
            }

            return Ok(Result.SuccessWithData(JsonConvert.SerializeObject(data)));
        }

        [HttpPost("Validate")]
        public IActionResult ValidateData([FromBody] ValidateResult products)
        {
            var invalidItems = _dataService.ValidateData<ProductTemplate>(products.DataJson);

            return Ok(invalidItems);
        }

        private static AttachmentDto InitAttachment(IFormFile file, string fileUrl, string fileName)
        {
            return new AttachmentDto
            {
                FileName = fileName,
                FileSize = file.Length,
                FileType = Path.GetExtension(file.FileName),
                FileUrl = fileUrl,
                FilePath = fileUrl,
            };
        }
    }
}
