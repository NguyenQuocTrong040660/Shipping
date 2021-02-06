using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.ManageUser.Commands
{
    public class UnlockUserCommand : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public UnlockUserCommandHandler(
             IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<Result> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
        {
            await _identityService.UnlockUserAsync(request.UserId);
            return Result.Success();
        }
    }
}
