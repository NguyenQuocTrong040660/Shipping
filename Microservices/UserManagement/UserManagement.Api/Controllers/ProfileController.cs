using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Results;
using UserManagement.Application.Profile.Commands;
using UserManagement.Application.Profile.Queries;

namespace UserManagement.Api.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public ProfileController(ILogger<ProfileController> logger,
            ICurrentUserService currentUserService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ChangePasswordRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await Mediator.Send(new UpdatePasswordCommand
            {
                CurrentUserId = _currentUserService.UserId,
                NewPassword = request.NewPassword,
                OldPassword = request.OldPassword
            });

            if (result.Succeeded)
            {
                _logger.LogInformation("User changed their password successfully.");
                return Ok(Result.Success());
            }

            return BadRequest(result);
        }

        [HttpPost("forget-password")]
        [ProducesResponseType(typeof(ForgetPasswordRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await Mediator.Send(new CreateNewPasswordCommand
            {
                Email = request.Email
            });

            _logger.LogInformation("User reset their password successfully.");
            return Ok(result);
        }

        [HttpGet("info")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResult>> UserInfo()
        {
            if (string.IsNullOrEmpty(_currentUserService.UserId))
            {
                return NotFound(Result.Failure("User were logout"));
            }

            var result = await Mediator.Send(new GetInfomationsUserQuery
            {
                UserId = _currentUserService.UserId
            });

            if (result == null)
            {
                return NotFound(Result.Failure("User were logout"));
            }

            return Ok(result);
        }
    }
}
