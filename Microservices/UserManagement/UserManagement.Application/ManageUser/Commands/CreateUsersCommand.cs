using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Extensions;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Models;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.ManageUser.Commands
{
    public class CreateUsersCommand : IRequest<List<CreateUserResult>>
    {
        public List<CreateUserRequest> Users { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUsersCommand, List<CreateUserResult>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            ILogger<CreateUserCommandHandler> logger,
             IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<CreateUserResult>> Handle(CreateUsersCommand request, CancellationToken cancellationToken)
        {
            List<CreateUserResult> createUserResults = new List<CreateUserResult>();

            foreach (var item in request.Users)
            {
                (var result, string temporaryPassword) = await _identityService.CreateUserWithTemporaryPasswordAsync(item.Email, item.UserName, item.RoleId);

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to create user with email {0}", item.Email);
                }

                createUserResults.Add(new CreateUserResult
                {
                    Email = item.Email,
                    Password = temporaryPassword.ToBase64Encode()
                });
            }

            return createUserResults;
        }
    }
}
