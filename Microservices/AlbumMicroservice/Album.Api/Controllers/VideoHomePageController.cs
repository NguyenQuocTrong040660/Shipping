using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Album.Domain.Models;
using Album.Application.YoutubeEmebed.Commands;
using Album.Application.YoutubeEmebed.Queries;
using Album.Application.Common.Interfaces;
using Album.Domain.Enumerations;
using System.IO;
using Album.Application.Attachments.Commands;

namespace Album.Api.Controllers
{
    public class VideoHomePageController : BaseController
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<VideoHomePageController> _logger;
        private readonly IUploadFileService _uploadService;

        public VideoHomePageController(IMediator mediator,
            IUploadFileService uploadService,
            IWebHostEnvironment environment, ILogger<VideoHomePageController> logger) : base(mediator)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VideoHomePageDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VideoHomePageDto>>> GetVideoHomePages()
        {
            var result = await _mediator.Send(new GetAllVideoHomePagesQuery());
            return Ok(result);
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(VideoHomePageDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> UpdateVideoHomePage(Guid key, [FromForm] VideoHomePageDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            if (model.File != null)
            {
                (Result resultUpload, string fileUrl, string fileName) = _uploadService
                    .UploadFile(model.File,
                                GetDomain(),
                                _environment.WebRootPath,
                                AttachmentTypes.Photo);
                
                if (resultUpload.Succeeded)
                {
                    model.YoutubeImage = fileUrl;

                    AttachmentDto attachment = InitAttachment(model.File, fileUrl, fileName, Guid.Empty.ToString());
                    await _mediator.Send(new AddAttachmentCommand() { Model = attachment });
                }

                _logger.LogInformation("Upload result", resultUpload);
            }

            var result = await _mediator.Send(new UpdateVideoHomePageCommand() { Id = key, Entity = model });

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(VideoHomePageDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> AddVideoHomePage([FromForm] VideoHomePageDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            if (model.File != null)
            {
                (Result resultUpload, string fileUrl, string fileName) = _uploadService
                    .UploadFile(model.File,
                                GetDomain(),
                                _environment.WebRootPath,
                                AttachmentTypes.Photo);

                if (resultUpload.Succeeded)
                {
                    model.YoutubeImage = fileUrl;

                    AttachmentDto attachment = InitAttachment(model.File, fileUrl, fileName, Guid.Empty.ToString());
                    await _mediator.Send(new AddAttachmentCommand() { Model = attachment });
                }

                _logger.LogInformation("Upload result", resultUpload);
            }

            var result = await _mediator.Send(new AddVideoHomePageCommand() { Model = model });

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result>> DeleteVideoHomePage(Guid id)
        {
            var result = await _mediator.Send(new DeleteVideoHomePageCommand() { Id = id });

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private AttachmentDto InitAttachment(IFormFile file, string fileUrl, string fileName, string attachmentTypeId)
        {
            return new AttachmentDto
            {
                FileName = fileName,
                FileSize = file.Length,
                FileType = Path.GetExtension(file.FileName),
                FileUrl = fileUrl,
                AttachmentTypeId = Guid.Parse(attachmentTypeId),
                FilePath = fileUrl,
            };
        }
    }
}
