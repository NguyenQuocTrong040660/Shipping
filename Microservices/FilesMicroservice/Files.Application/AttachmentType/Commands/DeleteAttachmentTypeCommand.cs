using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Files.Application.Common.Interfaces;
using Files.Domain.Models;

namespace Files.Application.AttachmentType.Commands
{
    public class DeleteAttachmentTypeCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAttachmentTypeCommandHandler : IRequestHandler<DeleteAttachmentTypeCommand, Result>
    {
        private readonly IFilesDbContext _context;
        private readonly IMapper _mapper;

        public DeleteAttachmentTypeCommandHandler(IFilesDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(DeleteAttachmentTypeCommand request, CancellationToken cancellationToken)
        {
            var entiy = _context.AttachmentTypes.Find(request.Id);

            if (entiy == null)
            {
               throw new ArgumentNullException(nameof(entiy));
            }

            _context.AttachmentTypes.Remove(entiy);

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to delete attachment type");
        }
    }
}
