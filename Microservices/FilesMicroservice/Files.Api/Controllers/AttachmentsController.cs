using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Files.Domain.Models;
using Files.Application.Common.Interfaces;
using System.IO;
using Files.Application.Attachments.Queries;
using Files.Application.Attachments.Commands;
using System.Collections.Generic;
using Files.Domain.Enumerations;

namespace Files.Api.Controllers
{
    public class AttachmentsController : BaseController
    {
        private readonly ILogger<AttachmentsController> _logger;
        private readonly IUploadFileService _uploadService;

        public AttachmentsController(IMediator mediator,
            IUploadFileService uploadService,
            ILogger<AttachmentsController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
        }

        [HttpGet("Photo")]
        [ProducesResponseType(typeof(List<AttachmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AttachmentDto>>> GetAllPhotoAttachments()
        {
            var result = await _mediator.Send(new GetAllAttachmentsQuery() { Type = AttachmentTypes.Photo });
            return Ok(result);
        }

        [HttpGet("Video")]
        [ProducesResponseType(typeof(List<AttachmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AttachmentDto>>> GetAllVideoAttachments()
        {
            var result = await _mediator.Send(new GetAllAttachmentsQuery() { Type = AttachmentTypes.Video });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result>> DeleteAttachments(Guid id)
        {
            var result = await _mediator.Send(new DeleteAttachmentCommand() { Id = id });

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("BulkDeleteAttachments")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result>> BulkDelete([FromBody]List<string> items)
        {
            var result = await _mediator.Send(new DeleteAttachmentsCommand() { Items = items });

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost, DisableRequestSizeLimit]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UploadFileRequest), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> AddAttachment([FromForm] UploadFileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            if (request.File == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            (Result resultUpload, string fileUrl, string fileName) = _uploadService
                    .UploadFileWithAttachmentTypeId(request.File,
                                GetDomain(),
                                request.AttachmentTypeId);

            if (!resultUpload.Succeeded)
            {
                return BadRequest("Failed to Upload Attachment");
            }

            AttachmentDto attachment = InitAttachment(request.File, fileUrl, fileName);
            _logger.LogInformation("Upload result", resultUpload);
            Result result = await Mediator.Send(new AddAttachmentCommand() { Model = attachment });

            return Ok(result);
        }

        [HttpPost("BulkInsertPhotos"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> BulkInsertPhotos([FromForm] List<IFormFile> files)
        {
            foreach (var item in files)
            {

                (Result resultUpload, string fileUrl, string fileName) = _uploadService
                        .UploadFile(item, GetDomain(), AttachmentTypes.Photo);

                if (resultUpload.Succeeded)
                {
                    AttachmentDto attachment = InitAttachment(item, fileUrl, fileName);
                    _logger.LogInformation("Upload result", resultUpload);
                    await Mediator.Send(new AddAttachmentCommand() { Model = attachment, AttachmentType = AttachmentTypes.Photo });
                }
            }

            return Ok();
        }

        [HttpPost("BulkInsertVideos"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> BulkInsertVideos([FromForm] List<IFormFile> files)
        {
            foreach (var item in files)
            {

                (Result resultUpload, string fileUrl, string fileName) = _uploadService
                        .UploadFile(item,
                                    GetDomain(),
                                    AttachmentTypes.Video);

                if (resultUpload.Succeeded)
                {
                    AttachmentDto attachment = InitAttachment(item, fileUrl, fileName);
                    _logger.LogInformation("Upload result", resultUpload);
                    await Mediator.Send(new AddAttachmentCommand() { Model = attachment, AttachmentType = AttachmentTypes.Video });
                }
            }

            return Ok();
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
