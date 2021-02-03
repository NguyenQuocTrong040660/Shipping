using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.User.Commands
{
    public class AddUserCommand : IRequest<Result>
    {
        public string Email { get;set; }
        public string Password { get; set; }
        
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(
             ILogger<AddUserCommandHandler> logger,
             IIdentityService identityService)
        {
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            (Result result, string userId) = await _identityService.CreateUserAsync(request.Email, request.Password);

            return result;
        }
    }
}
