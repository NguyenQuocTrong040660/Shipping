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
    public class GetUserLoginResultQuery : IRequest<IdentityResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class GetUserLoginResultQueryHandler : IRequestHandler<GetUserLoginResultQuery, IdentityResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<GetUserLoginResultQueryHandler> _logger;
        private readonly IJwtAuthManager _jwtAuthManager;

        public GetUserLoginResultQueryHandler(
             ILogger<GetUserLoginResultQueryHandler> logger,
             IIdentityService identityService,
             IJwtAuthManager jwtAuthManager)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(identityService));
            _jwtAuthManager = jwtAuthManager ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<IdentityResult> Handle(GetUserLoginResultQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SignInAsync(request.UserName, request.Password, request.RememberMe);
            var user = await _identityService.GetUserByIdentifierAsync(request.UserName);

            if (result.IsLockedOut)
            {
                return IdentityResult.LockedOut();
            }

            if (result.Succeeded)
            {
                if (user.RequireChangePassword)
                {
                    return IdentityResult.MustChangePassword();
                }

                var roles = await _identityService.GetRolesUserAsync(request.UserName);
                var userId = await _identityService.GetUserIdAsync(request.UserName);
                await _identityService.ResetAccessFailedCountAsync(userId);

                List<Claim> claims = BuidUserClaims(request.UserName, userId, roles);

                var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims.ToArray(), DateTime.Now);

                _logger.LogInformation($"User [{request.UserName}] logged in the system.");

                return IdentityResult.Success(request.UserName, roles, request.UserName, jwtResult.AccessToken, jwtResult.RefreshToken.TokenString);
            }

            return IdentityResult.Error("Username or Password incorrect");
        }

        private static List<Claim> BuidUserClaims(string userName, string userId, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            return claims;
        }
    }
}
