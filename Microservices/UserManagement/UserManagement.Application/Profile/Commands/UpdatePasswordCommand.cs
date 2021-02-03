using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.Profile.Commands
{
    public class UpdatePasswordCommand : IRequest<Result>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string CurrentUserId { get; set; }
    }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Result>
    {
        private readonly IIdentityService _identityService;
        
        public UpdatePasswordCommandHandler(
             IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangePasswordAsync(
                request.CurrentUserId,
                request.OldPassword,
                request.NewPassword);

            return result;
        }
    }
}
