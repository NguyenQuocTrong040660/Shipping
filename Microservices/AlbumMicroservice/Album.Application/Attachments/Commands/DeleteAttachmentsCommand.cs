using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Album.Application.Common.Interfaces;
using Album.Domain.Models;
using System.Collections.Generic;
using Album.Domain.Entities;

namespace Album.Application.Attachments.Commands
{
    public class DeleteAttachmentsCommand : IRequest<Result>
    {
        public List<string> Items { get; set; }
        public string WebRootPath { get; set; }
    }

    public class DeleteAttachmentsCommandHandler : IRequestHandler<DeleteAttachmentsCommand, Result>
    {
        private readonly IAlbumDbContext _context;
        private readonly IUploadFileService _fileService;
        private readonly IMapper _mapper;

        public DeleteAttachmentsCommandHandler(IAlbumDbContext context,
            IUploadFileService fileService,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService;
        }

        public async Task<Result> Handle(DeleteAttachmentsCommand request, CancellationToken cancellationToken)
        {
            var entites = new List<Attachment>();
            
            foreach (var item in request.Items)
            {
                var entity = _context.Attachments.Find(new Guid(item));
                entites.Add(entity);
            }

            _context.Attachments.RemoveRange(entites);

            foreach (var item in entites)
            {
                _fileService.DeleteFile(request.WebRootPath, item.AttachmentTypeId.ToString(), item.FileName);
            }

            return await _context.SaveChangesAsync() > 0 
                ? Result.Success()
                : Result.Failure("Failed to delete attachment");
        }
    }
}
