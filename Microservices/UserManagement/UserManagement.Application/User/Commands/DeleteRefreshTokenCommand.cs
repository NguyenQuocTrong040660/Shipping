using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Application.User.Commands
{
    public class DeleteRefreshTokenCommand : IRequest
    {
        public string UserName { get; set; }
    }

    public class DeleteRefreshTokenCommandHandler : IRequestHandler<DeleteRefreshTokenCommand, Unit>
    {
        private readonly ILogger<DeleteRefreshTokenCommandHandler> _logger;
        private readonly IJwtAuthManager _jwtAuthManager;

        public DeleteRefreshTokenCommandHandler(
             ILogger<DeleteRefreshTokenCommandHandler> logger,
             IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _jwtAuthManager.RemoveRefreshTokenByUserName(request.UserName);
            return await Task.FromResult(Unit.Value);
        }
    }
}
