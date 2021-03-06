using AutoMapper;
using MediatR;
using System;
using Models = Files.Domain.Models;
using Entities = Files.Domain.Entities;
using System.Threading;
using Files.Application.Common.Exceptions;
using System.Threading.Tasks;
using Files.Application.Common.Interfaces;
using Files.Domain.Models;

namespace Files.Application.Attachments.Commands
{
    public class UpdateAttachmentCommand : IRequest<Result>
    {
        public Models.AttachmentDto Entity { get; set; }
        public Guid Id { get; set; }
    }

    public class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, Result>
    {
        private readonly IFilesDbContext _context;
        private readonly IMapper _mapper;

        public UpdateAttachmentCommandHandler(IFilesDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Attachments.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(entity), request.Id);
            }

            entity.FileType = request.Entity.FileType;
            entity.FileSize = request.Entity.FileSize;
            entity.FilePath = request.Entity.FilePath;
            entity.FileName = request.Entity.FileName;
            entity.FileUrl = request.Entity.FileUrl;
            entity.AttachmentTypeId = request.Entity.AttachmentTypeId;

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to update attachment");
        }
    }
}
