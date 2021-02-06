using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.ManageUser.Commands
{
    public class CreateUserCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(
             IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.CreateUserWithTemporaryPasswordAsync(request.Email);
        }
    }
}
