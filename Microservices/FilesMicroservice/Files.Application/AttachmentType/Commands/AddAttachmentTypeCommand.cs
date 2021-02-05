using AutoMapper;
using System;
using Models = Files.Domain.Models;
using Entities = Files.Domain.Entities;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Files.Application.Common.Interfaces;
using Files.Domain.Models;

namespace Files.Application.AttachmentType.Commands
{
    public class AddAttachmentTypeCommand : IRequest<Result>
    {
        public Models.AttachmentTypeDto Model { get; set; }
    }

    public class AddAttachmentTypeCommandHandler : IRequestHandler<AddAttachmentTypeCommand, Result>
    {
        private readonly IFilesDbContext _context;
        private readonly IMapper _mapper;

        public AddAttachmentTypeCommandHandler(IFilesDbContext context, IMapper mapper)
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
