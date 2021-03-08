using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
    [Authorize(Roles = "SystemAdministrator,ITAdministrator")]
    public class AdminController : ApiController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public AdminController(ILogger<AdminController> logger,
            ICurrentUserService currentUserService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpGet]
        public IActionResult CheckAdminCanAccess()
        {
            _logger.LogInformation("Admin access successfully");
            return Ok();
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(List<UserResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<UserResult>>> RetriveAllUsersAsync()
        {
            var users = await Mediator.Send(new GetAllUserQuery { });
            users.RemoveAll(i => i.Id == _currentUserService.UserId);
            return Ok(users);
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(List<CreateUserResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<CreateUserResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<CreateUserResult>>> CreateUserAsync([FromBody] List<CreateUserRequest> createUsersRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(createUsersRequest);
            }

            return Ok(await Mediator.Send(new CreateUsersCommand { Users = createUsersRequest }));
        }

        [HttpPost("users/reset-password")]
        [ProducesResponseType(typeof(List<ResetPasswordResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ResetPasswordResult>>> ResetPasswordUsersAsync([FromBody] List<string> userEmails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userEmails);
            }

            return Ok(await Mediator.Send(new ResetUsersPasswordCommand { UserEmails = userEmails }));
        }

        [HttpPost("users/lock")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> LockUsersAsync([FromBody] List<string> users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(users);
            }

            return Ok(await Mediator.Send(new LockUsersCommand { Users = users }));
        }

        [HttpPost("user/lock")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> LockUserAsync([FromBody] LockRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            Result result = await Mediator.Send(new LockUserCommand {
                UserId = request.UserId
            });
       
            return Ok(result);
        }

        [HttpPost("user/unlock")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result>> UnlockUserAsync([FromBody] LockRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            Result result = await Mediator.Send(new UnlockUserCommand
            {
                UserId = request.UserId
            });

            return Ok(result);
        }

        [HttpGet("roles")]
        [ProducesResponseType(typeof(List<RoleModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<RoleModel>>> GetRolesAsync()
        {
            var roles = await Mediator.Send(new GetRolesQuery { });
            roles.RemoveAll(i => i.Name.Equals(Roles.SystemAdministrator));
            return Ok(roles);
        }
    }
}
