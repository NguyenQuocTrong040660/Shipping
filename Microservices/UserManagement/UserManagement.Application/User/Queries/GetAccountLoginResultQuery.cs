using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.User.Queries
{
    public class GetUserLoginResultQuery : IRequest<LoginResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class GetUserLoginResultQueryHandler : IRequestHandler<GetUserLoginResultQuery, LoginResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<GetUserLoginResultQueryHandler> _logger;
        private readonly IJwtAuthManager _jwtAuthManager;

        public GetUserLoginResultQueryHandler(
             ILogger<GetUserLoginResultQueryHandler> logger,
             IIdentityService identityService,
             IJwtAuthManager jwtAuthManager)
        {
            _identityService = identityService;
            _logger = logger;
            _jwtAuthManager = jwtAuthManager;
        }

        public async Task<LoginResult> Handle(GetUserLoginResultQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SignInAsync(request.UserName, request.Password, request.RememberMe);

            if (result.Succeeded)
            {
                var roles = await _identityService.GetRolesUserAsync(request.UserName);
                var userId = await _identityService.GetUserIdAsync(request.UserName);

                await _identityService.ResetAccessFailedCountAsync(userId);

                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, request.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

                foreach (var item in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }

                var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims.ToArray(), DateTime.Now);

                _logger.LogInformation($"User [{request.UserName}] logged in the system.");

                return LoginResult.Success(request.UserName, roles, request.UserName, jwtResult.AccessToken, jwtResult.RefreshToken.TokenString);
            }

            if (result.IsLockedOut)
            {
                return LoginResult.LockedOut();
            }

            return LoginResult.Error("UserName or Password incorrect");
        }
    }
}
