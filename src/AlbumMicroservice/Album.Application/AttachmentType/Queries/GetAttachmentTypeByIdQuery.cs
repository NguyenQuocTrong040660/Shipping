using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Models = Album.Domain.Models;
using Album.Application.Common.Interfaces;

namespace Album.Application.AttachmentType.Queries
{
    public class GetAttachmentTypeByIdQuery : IRequest<Models.AttachmentTypeDto>
    {
        public Guid Id;
    }

    public class GetAttachmentTypeByIdQueryHandler : IRequestHandler<GetAttachmentTypeByIdQuery, Models.AttachmentTypeDto>
    {
        private readonly IAlbumDbContext _context;
        private readonly IMapper _mapper;

        public GetAttachmentTypeByIdQueryHandler(IAlbumDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Models.AttachmentTypeDto> Handle(GetAttachmentTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var entiy = _context.AttachmentTypes.Find(request.Id);

            if (entiy == null)
            {
                throw new ArgumentNullException(nameof(entiy));
            }

            return await Task.FromResult(_mapper.Map<Models.AttachmentTypeDto>(entiy));
        }
    }
}
