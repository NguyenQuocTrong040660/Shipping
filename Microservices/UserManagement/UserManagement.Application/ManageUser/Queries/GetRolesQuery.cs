using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Results;
using UserManagement.Domain.Common;

namespace UserManagement.Application.ManageUser.Queries
{
    public class GetRolesQuery : IRequest<List<RoleModel>>
    {
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleModel>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetRolesQueryHandler(
            IMapper mapper,
            IApplicationDbContext context,
            ILogger<GetRolesQueryHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<RoleModel>> Handle(GetRolesQuery request, CancellationToken cancellationToken = new CancellationToken())
        {
            _logger.LogInformation("Get roles successfully");
            return _mapper.Map<List<RoleModel>>(await _context.Roles.AsNoTracking().ToListAsync());
        }
    }
}
