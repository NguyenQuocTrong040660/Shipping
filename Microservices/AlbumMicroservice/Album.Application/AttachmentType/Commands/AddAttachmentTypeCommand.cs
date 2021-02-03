using AutoMapper;
using System;
using Models = Album.Domain.Models;
using Entities = Album.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Album.Application.Common.Interfaces;
using Album.Domain.Models;

namespace Album.Application.AttachmentType.Commands
{
    public class AddAttachmentTypeCommand : IRequest<Result>
    {
        public Models.AttachmentTypeDto Model { get; set; }
    }

    public class AddAttachmentTypeCommandHandler : IRequestHandler<AddAttachmentTypeCommand, Result>
    {
        private readonly IAlbumDbContext _context;
        private readonly IMapper _mapper;

        public AddAttachmentTypeCommandHandler(IAlbumDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(AddAttachmentTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.AttachmentType>(request.Model);
            _context.AttachmentTypes.Add(entity);

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to add attachment type");
        }
    }
}
