using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;

namespace UserManagement.Application.ManageUser.Queries
{
    public class GetAllUserQuery : IRequest<List<UserResult>>
    {
        public string CurrentUserId { get; set; }
    }

    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<UserResult>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<GetAllUserQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetAllUserQueryHandler(
            IMapper mapper,
            ILogger<GetAllUserQueryHandler> logger,
            IIdentityService identityService)
        {
            _identityService = identityService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<UserResult>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CurrentUserId))
            {
                return new List<UserResult>();
            }

            var users = await _identityService.GetUsersAsync();

            return _mapper.Map<List<UserResult>>(users);
        }
    }
}
