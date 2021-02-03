using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Album.Application.Common.Interfaces;
using Album.Domain.Models;

namespace Album.Application.YoutubeEmebed.Commands
{
    public class DeleteVideoHomePageCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }

    public class DeleteVideoHomePageCommandHandler : IRequestHandler<DeleteVideoHomePageCommand, Result>
    {
        private readonly IAlbumDbContext _context;
        private readonly IMapper _mapper;

        public DeleteVideoHomePageCommandHandler(IAlbumDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(DeleteVideoHomePageCommand request, CancellationToken cancellationToken)
        {
            var entiy = _context.VideoHomePages.Find(request.Id);

            if (entiy == null)
            {
                throw new ArgumentNullException(nameof(entiy));
            }

            _context.VideoHomePages.Remove(entiy);
            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to delete VideoHomePage");
        }
    }
}
