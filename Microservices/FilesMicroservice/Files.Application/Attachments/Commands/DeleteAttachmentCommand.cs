using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Files.Application.Common.Interfaces;
using Files.Domain.Models;

namespace Files.Application.Attachments.Commands
{
    public class DeleteAttachmentCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string WebRootPath { get; set; }
    }

    public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, Result>
    {
        private readonly IFilesDbContext _context;
        private readonly IUploadFileService _fileService;
        private readonly IMapper _mapper;

        public DeleteAttachmentCommandHandler(IFilesDbContext context,
            IUploadFileService fileService,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService;
        }

        public async Task<Result> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entiy = _context.Attachments.Find(request.Id);

            if (entiy == null)
            {
                throw new ArgumentNullException(nameof(entiy));
            }

            _context.Attachments.Remove(entiy);

            _fileService.DeleteFile(request.WebRootPath, entiy.AttachmentTypeId.ToString(), entiy.FileName);

            return await _context.SaveChangesAsync() > 0 
                ? Result.Success()
                : Result.Failure("Failed to delete attachment");
        }
    }
}
