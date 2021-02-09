using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;
using UserManagement.Domain.Common;

namespace UserManagement.Application.Profile.Queries
{
    public class GetInfomationsUserQuery : IRequest<UserResult>
    {
        public string UserId { get; set; }
    }

    public class GetInfomationsUserQueryHandler : IRequestHandler<GetInfomationsUserQuery, UserResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<GetInfomationsUserQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetInfomationsUserQueryHandler(
            IMapper mapper,
            ILogger<GetInfomationsUserQueryHandler> logger,
            IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserResult> Handle(GetInfomationsUserQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return null;
            }

            var user = _mapper.Map<UserResult>(await _identityService.GetUserByIdentifierAsync(request.UserId));
            user.RoleName = await _identityService.GetRoleUserAsync(user.UserName);
            return user;
        }
    }
}
