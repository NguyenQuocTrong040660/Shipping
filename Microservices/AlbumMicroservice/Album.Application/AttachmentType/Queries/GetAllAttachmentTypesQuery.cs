using MediatR;
using System;
using System.Collections.Generic;
using Models = Album.Domain.Models;
using System.Threading.Tasks;
using Album.Application.Common.Interfaces;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Album.Application.AttachmentType.Queries
{
    public class GetAllAttachmentTypesQuery : IRequest<List<Models.AttachmentTypeDto>>
    {
    }

    public class GetAllAttachmentTypesQueryHandler : IRequestHandler<GetAllAttachmentTypesQuery, List<Models.AttachmentTypeDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAlbumDbContext _context;

        public GetAllAttachmentTypesQueryHandler(IAlbumDbContext context, IMapper mapper)
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
