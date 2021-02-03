using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Models = Album.Domain.Models;
using System.Collections.Generic;
using Album.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Album.Application.YoutubeEmebed.Queries
{
    public class GetAllVideoHomePagesQuery : IRequest<List<Models.VideoHomePageDto>>
    {
    }

    public class GetAllVideoHomePagesQueryHandler : IRequestHandler<GetAllVideoHomePagesQuery, List<Models.VideoHomePageDto>>
    {
        private readonly IAlbumDbContext _context;
        private readonly IMapper _mapper;

        public GetAllVideoHomePagesQueryHandler(IAlbumDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<Models.VideoHomePageDto>> Handle(GetAllVideoHomePagesQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.VideoHomePages.AsNoTracking().ToListAsync();
            return await Task.FromResult(_mapper.Map<List<Models.VideoHomePageDto>>(result));
        }
    }
}
