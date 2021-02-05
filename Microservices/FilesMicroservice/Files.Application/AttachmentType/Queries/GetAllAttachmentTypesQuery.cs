using MediatR;
using System;
using System.Collections.Generic;
using Models = Files.Domain.Models;
using System.Threading.Tasks;
using Files.Application.Common.Interfaces;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Files.Application.AttachmentType.Queries
{
    public class GetAllAttachmentTypesQuery : IRequest<List<Models.AttachmentTypeDto>>
    {
    }

    public class GetAllAttachmentTypesQueryHandler : IRequestHandler<GetAllAttachmentTypesQuery, List<Models.AttachmentTypeDto>>
    {
        private readonly IMapper _mapper;
        private readonly IFilesDbContext _context;

        public GetAllAttachmentTypesQueryHandler(IFilesDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<Models.AttachmentTypeDto>> Handle(GetAllAttachmentTypesQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.AttachmentTypes.AsNoTracking().ToListAsync();
            return await Task.FromResult(_mapper.Map<List<Models.AttachmentTypeDto>>(result));
        }
    }
}
