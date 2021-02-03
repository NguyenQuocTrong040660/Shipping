using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
            _identityService = identityService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserResult> Handle(GetInfomationsUserQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return null;
            }

            var user = await _identityService.GetUserByIdAsync(request.UserId);

            return _mapper.Map<UserResult>(user);
        }
    }
}
