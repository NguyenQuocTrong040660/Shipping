using AutoMapper;
using System;
using Models = Album.Domain.Models;
using Entities = Album.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Album.Application.Common.Interfaces;
using Album.Domain.Models;
using System.Linq;
using Album.Domain.Enumerations;

namespace Album.Application.Attachments.Commands
{
    public class AddAttachmentCommand : IRequest<Result>
    {
        public Models.AttachmentDto Model { get; set; }
        public string AttachmentType { get; set; }
    }

    public class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand, Result>
    {
        private readonly IAlbumDbContext _context;
        private readonly IMapper _mapper;

        public AddAttachmentCommandHandler(IAlbumDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.Attachment>(request.Model);

            entity.AttachmentType = null;

            Entities.AttachmentType attachmentType = null;

            switch (request.AttachmentType)
            {
                case AttachmentTypes.Video:
                    attachmentType = _context.AttachmentTypes.FirstOrDefault(x => x.Name == AttachmentTypes.Video);
                    break;
                case AttachmentTypes.Photo:
                    attachmentType = _context.AttachmentTypes.FirstOrDefault(x => x.Name == AttachmentTypes.Photo);
                    break;
                default:
                    throw new ArgumentNullException(nameof(attachmentType));
            }

            entity.AttachmentTypeId = attachmentType.Id;
            _context.Attachments.Add(entity);
            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to add attachment");
        }
    }
}
