using AutoMapper;
using MediatR;
using System;
using Models = Files.Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using Files.Application.Common.Interfaces;
using Files.Domain.Models;

namespace Files.Application.AttachmentType.Commands
{
    public class UpdateAttachmentTypeCommand : IRequest<Result>
    {
        public Models.AttachmentTypeDto Entity { get; set; }
        public Guid Id { get; set; }
    }

    public class UpdateAttachmentTypeCommandHandler : IRequestHandler<UpdateAttachmentTypeCommand, Result>
    {
        private readonly IFilesDbContext _context;
        private readonly IMapper _mapper;

        public UpdateAttachmentTypeCommandHandler(IFilesDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(UpdateAttachmentTypeCommand request, CancellationToken cancellationToken)
        {
            var entiy = _context.AttachmentTypes.Find(request.Id);

            if (entiy == null)
            {
                throw new ArgumentNullException(nameof(entiy));
            }

            entiy.Name = request.Entity.Name;

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to update attachment type");
        }
    }
}
