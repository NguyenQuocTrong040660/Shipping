using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Files.Domain.Models;
using Files.Application.AttachmentType.Queries;
using Files.Application.AttachmentType.Commands;

namespace Files.Api.Controllers
{
    public class AttachmentTypeController : BaseController
    {
        private readonly ILogger<AttachmentTypeController> _logger;

        public AttachmentTypeController(IMediator mediator,
          ILogger<AttachmentTypeController> logger) : base(mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AttachmentTypeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AttachmentTypeDto>>> GetAllAttachmentTypes()
        {
            var result = await _mediator.Send(new GetAllAttachmentTypesQuery());
            return Ok(result);
        }

        [HttpGet("Detail/{id}")]
        [ProducesResponseType(typeof(AttachmentTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AttachmentTypeDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AttachmentTypeDto>> GetAttachmentType(Guid id)
        {
            var result = await _mediator.Send(new GetAttachmentTypeByIdQuery() { Id = id });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{key}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AttachmentTypeDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> UpdateAttachmentType(Guid key, [FromBody] AttachmentTypeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _mediator.Send(new UpdateAttachmentTypeCommand() { Id = key, Entity = model });

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(AttachmentTypeDto), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> AddAttachmentType([FromBody] AttachmentTypeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await _mediator.Send(new AddAttachmentTypeCommand() { Model = model });

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
            var result = await _mediator.Send(new DeleteAttachmentTypeCommand() { Id = id });

            if (!result.Succeeded)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
