using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.ManageUser.Commands
{
    public class LockUserCommand : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class LockUserCommandHandler : IRequestHandler<LockUserCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public LockUserCommandHandler(
             IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(LockUserCommand request, CancellationToken cancellationToken)
        {
            await _identityService.LockUserAsync(request.UserId);
            return Result.Success();
        }
    }
}
