using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Models = Files.Domain.Models;
using Files.Application.Common.Interfaces;

namespace Files.Application.AttachmentType.Queries
{
    public class GetAttachmentTypeByIdQuery : IRequest<Models.AttachmentTypeDto>
    {
        public Guid Id;
    }

    public class GetAttachmentTypeByIdQueryHandler : IRequestHandler<GetAttachmentTypeByIdQuery, Models.AttachmentTypeDto>
    {
        private readonly IFilesDbContext _context;
        private readonly IMapper _mapper;

        public GetAttachmentTypeByIdQueryHandler(IFilesDbContext context, IMapper mapper)
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
