using AutoMapper;
using System;
using Models = Files.Domain.Models;
using Entities = Files.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Files.Application.Common.Interfaces;
using Files.Domain.Models;
using System.Linq;
using Files.Domain.Enumerations;

namespace Files.Application.Attachments.Commands
{
    public class AddAttachmentCommand : IRequest<Result>
    {
        public Models.AttachmentDto Model { get; set; }
        public string AttachmentType { get; set; }
    }

    public class AddAttachmentCommandHandler : IRequestHandler<AddAttachmentCommand, Result>
    {
        private readonly IFilesDbContext _context;
        private readonly IMapper _mapper;

        public AddAttachmentCommandHandler(IFilesDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Entities.Attachment>(request.Model);
            entity.AttachmentType = null;

            Entities.AttachmentType attachmentType = attachmentType = _context.AttachmentTypes.FirstOrDefault(x => x.Name == request.AttachmentType);

            if (attachmentType == null)
            {
                throw new ArgumentNullException(nameof(attachmentType));
            }

            entity.AttachmentTypeId = attachmentType.Id;
            _context.Attachments.Add(entity);
            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to add attachment");
        }
    }
}
