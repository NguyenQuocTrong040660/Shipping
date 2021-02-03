﻿using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.User.Queries
{
    public class GetNewTokenByRefreshTokenQuery : IRequest<LoginResult>
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
    }

    public class GetNewTokenByRefreshTokenQueryHandler : IRequestHandler<GetNewTokenByRefreshTokenQuery, LoginResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<GetUserLoginResultQueryHandler> _logger;
        private readonly IJwtAuthManager _jwtAuthManager;

        public GetNewTokenByRefreshTokenQueryHandler(
             ILogger<GetUserLoginResultQueryHandler> logger,
             IIdentityService identityService,
             IJwtAuthManager jwtAuthManager)
        {
            _identityService = identityService;
            _logger = logger;
            _jwtAuthManager = jwtAuthManager;
        }

        public async Task<LoginResult> Handle(GetNewTokenByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, request.AccessToken, DateTime.Now);
                var roles = await _identityService.GetRolesUserAsync(request.UserName);

                _logger.LogInformation($"User [{request.UserName}] has refreshed JWT token.");

                return LoginResult.Success(request.UserName, roles, request.UserName, jwtResult.AccessToken, jwtResult.RefreshToken.TokenString);
            }
            catch (SecurityTokenException e)
            {
                throw e;
            }
        }
    }
}
