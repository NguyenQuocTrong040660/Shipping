using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Models = Album.Domain.Models;
using Album.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Album.Application.YoutubeEmebed.Queries
{
    public class GetVideoHomePageByIdQuery : IRequest<Models.VideoHomePageDto>
    {
        public Guid Id;
    }

    public class GetVideoHomePageByIdQueryHandler : IRequestHandler<GetVideoHomePageByIdQuery, Models.VideoHomePageDto>
    {
        private readonly IAlbumDbContext _context;
        private readonly IMapper _mapper;

        public GetVideoHomePageByIdQueryHandler(IAlbumDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Models.VideoHomePageDto> Handle(GetVideoHomePageByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.VideoHomePages.SingleOrDefaultAsync(i => i.Id == request.Id);
            return await Task.FromResult(_mapper.Map<Models.VideoHomePageDto>(result));
        }
    }
}
