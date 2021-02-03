using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;
using UserManagement.Application.Common.Models;
using UserManagement.Application.User.Commands;
using UserManagement.Application.User.Queries;
using System.Security.Claims;
using System.Linq;

namespace UserManagement.Api.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly ILogger<UserController> _logger;
        private readonly ICurrentUserService _userService;

        public UserController(ILogger<UserController> logger, 
            ICurrentUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            LoginResult result = await Mediator.Send(new GetUserLoginResultQuery
            {
                UserName = request.UserName,
                Password = request.Password,
                RememberMe = request.RememberMe,
            });

            if (result == null)
                return Unauthorized();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            Result result = await Mediator.Send(new AddUserCommand
            {
                Email = request.Email,
                Password = request.Password
                
            });

            if (result.Succeeded)
            {
                _logger.LogInformation($"User [{request.Email}] register successfully.");

                return Ok(result);
            }

            return BadRequest(Result.Failure("Register failed"));
        }

        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var userName = User.Identity.Name;

            await Mediator.Send(new DeleteRefreshTokenCommand
            {
                UserName = userName
            });

            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResult>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity.Name;

                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                string accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");

                LoginResult result = await Mediator.Send(new GetNewTokenByRefreshTokenQuery
                {
                    AccessToken = accessToken,
                    RefreshToken = request.RefreshToken,
                    UserName = userName
                });

                return Ok(result);
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }

        [HttpGet("info")]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public ActionResult GetUserInfo()
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return Ok(new LoginResult
            {
                UserName = User.Identity.Name,
                Roles = roles,
                OriginalUserName = User.FindFirst("OriginalUserName")?.Value
            });
        }
    }
}
