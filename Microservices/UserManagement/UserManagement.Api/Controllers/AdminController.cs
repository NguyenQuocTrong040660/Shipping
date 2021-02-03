using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Results;
using UserManagement.Application.ManageUser.Commands;
using UserManagement.Application.ManageUser.Queries;
using UserManagement.Domain.Enums;

namespace UserManagement.Api.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class AdminController: ApiController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ICurrentUserService _userService;

        public AdminController(ILogger<AdminController> logger,
            ICurrentUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult CheckAdminCanAccess()
        {
            _logger.LogInformation("Admin access successfully");
            return Ok();
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(List<UserResult>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserResult>>> RetriveAllUsers()
        {
            var users = await Mediator.Send(new GetAllUserQuery
            {
                CurrentUserId = _userService.UserId
            });

            return Ok(users);
        }

        [HttpPost("lock")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> LockUser([FromBody] LockRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            Result result = await Mediator.Send(new LockUserCommand {
                UserId = request.UserId
            });

            if (result == null)
                return BadRequest(Result.Failure("LockUser failed"));

            return Ok(result);
        }

        [HttpPost("unlock")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UnlockUser([FromBody] LockRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            Result result = await Mediator.Send(new UnlockUserCommand
            {
                UserId = request.UserId
            });

            if (result == null)
                return BadRequest(Result.Failure("LockUser failed"));

            return Ok(result);
        }
    }
}
