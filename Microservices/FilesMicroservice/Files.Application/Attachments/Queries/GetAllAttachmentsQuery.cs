using MediatR;
using System;
using System.Collections.Generic;
using Models = Files.Domain.Models;
using System.Threading.Tasks;
using Files.Application.Common.Interfaces;
using AutoMapper;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Files.Domain.Enumerations;

namespace Files.Application.Attachments.Queries
{
    public class GetAllAttachmentsQuery : IRequest<List<Models.AttachmentDto>>
    {
        public string Type { get; set; }
    }

    public class GetAllAttachmentsQueryHandler : IRequestHandler<GetAllAttachmentsQuery, List<Models.AttachmentDto>>
    {
        private readonly IMapper _mapper;
        private readonly IFilesDbContext _context;

        public GetAllAttachmentsQueryHandler(IFilesDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<Models.AttachmentDto>> Handle(GetAllAttachmentsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Attachments
                .Include(x => x.AttachmentType)
                .OrderByDescending(x => x.Created)
                .AsQueryable();

            if (string.Equals(request.Type, AttachmentTypes.Photo))
            {
                query = query.Where(e => string.Equals(e.AttachmentType.Name, AttachmentTypes.Photo));
            }

            if (string.Equals(request.Type, AttachmentTypes.Video))
            {
                query = query.Where(e => string.Equals(e.AttachmentType.Name, AttachmentTypes.Video));
            }

            var result = await query.AsNoTracking().ToListAsync();

            return await Task.FromResult(_mapper.Map<List<Models.AttachmentDto>>(result));
        }
    }
}
