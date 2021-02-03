using AutoMapper;
using MediatR;
using System;
using Models = Album.Domain.Models;
using Entities = Album.Domain.Entities;
using System.Threading;
using Album.Application.Common.Exceptions;
using System.Threading.Tasks;
using Album.Application.Common.Interfaces;
using Album.Domain.Models;
using System.Text.RegularExpressions;
using System.Linq;

namespace Album.Application.YoutubeEmebed.Commands
{
    public class UpdateVideoHomePageCommand : IRequest<Result>
    {
        public Models.VideoHomePageDto Entity { get; set; }
        public Guid Id { get; set; }
    }

    public class UpdateVideoHomePageCommandHandler : IRequestHandler<UpdateVideoHomePageCommand, Result>
    {
        private readonly IAlbumDbContext _context;
        private readonly IMapper _mapper;

        private const string YoutubeLinkRegex = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";
        private static Regex regexExtractId = new Regex(YoutubeLinkRegex, RegexOptions.Compiled);
        private static string[] validAuthorities = { "youtube.com", "www.youtube.com", "youtu.be", "www.youtu.be" };

        public UpdateVideoHomePageCommandHandler(IAlbumDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(UpdateVideoHomePageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.VideoHomePages.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(entity), request.Id);
            }

            entity.Code = request.Entity.Code;
            entity.Descriptions = request.Entity.Descriptions;
            entity.Height = request.Entity.Height;
            entity.Width = request.Entity.Width;
            entity.YoutubeLink = request.Entity.YoutubeLink;
            entity.YoutubeId = GetYoutubeId(entity.YoutubeLink) == null ? entity.YoutubeId : GetYoutubeId(entity.YoutubeLink);
            entity.YoutubeImage = request.Entity.YoutubeImage;

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Failed to update VideoHomePage");
        }

        private string GetYoutubeId(string uri)
        {
            try
            {
                string authority = new UriBuilder(uri).Uri.Authority.ToLower();

                //check if the url is a youtube url
                if (validAuthorities.Contains(authority))
                {
                    //and extract the id
                    var regRes = regexExtractId.Match(uri.ToString());
                    if (regRes.Success)
                    {
                        return regRes.Groups[1].Value;
                    }
                }
            }
            catch { }


            return null;
        }
    }
}
