using Album.Domain.Entities;
using Album.Domain.Enumerations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Persistence.DBContext
{
    public static class AlbumDbContextSeed
    {
        public static async Task SeedVideoHomePagesAsync(AlbumDbContext context)
        {
            // Seed, if necessary
            if (!context.VideoHomePages.Any())
            {
                var videoHomePages = new List<VideoHomePage>
                {
                    new VideoHomePage
                    {
                        Descriptions = "Họp báo công bố cuộc thi Miss Tourism Vietnam 2020",
                        Code = "",
                        Height = 0,
                        Width = 0,
                        YoutubeId = "o0gLZF4NpwM",
                        YoutubeLink = "https://www.youtube.com/watch?v=o0gLZF4NpwM&feature=emb_title",
                        YoutubeImage = string.Empty
                    },
                    new VideoHomePage
                    {
                        Descriptions = "Khởi động cuộc thi Hoa khôi du lịch Việt Nam 2020 - PLO",
                        Code = "",
                        Height = 0,
                        Width = 0,
                        YoutubeId = "W2dNeXdE0B0",
                        YoutubeLink = "https://www.youtube.com/watch?v=W2dNeXdE0B0",
                        YoutubeImage = string.Empty
                    },
                    new VideoHomePage
                    {
                        Descriptions = "Bản tin HTV - Hop bao Hoa khoi du lich Viet Nam MISS TOURISM 2020",
                        Code = "",
                        Height = 0,
                        Width = 0,
                        YoutubeId = "eBF21G_rqnY",
                        YoutubeLink = "https://www.youtube.com/watch?v=eBF21G_rqnY&feature=emb_title",
                        YoutubeImage = string.Empty
                    },
                    new VideoHomePage
                    {
                        Descriptions = "Miss Tourism Vietnam 2020 sẽ diễn ra tại Đắk Nông",
                        Code = "",
                        Height = 0,
                        Width = 0,
                        YoutubeId = "BY04WPQxu70",
                        YoutubeLink = "https://www.youtube.com/watch?v=BY04WPQxu70&feature=emb_title",
                        YoutubeImage = string.Empty
                    },
                    new VideoHomePage
                    {
                        Descriptions = "Miss Tourism Viet Nam 2020 - Thanh Âm Trái Đất – Sound Of The Earth",
                        Code = "",
                        Height = 0,
                        Width = 0,
                        YoutubeId = "QpbWMjNWr4o",
                        YoutubeLink = "https://www.youtube.com/watch?v=QpbWMjNWr4o&feature=emb_title",
                        YoutubeImage = string.Empty
                    },
                    new VideoHomePage
                    {
                        Descriptions = "[FULL] Đêm chung kết HOA KHÔI DU LỊCH VIỆT NAM 2017",
                        Code = "",
                        Height = 0,
                        Width = 0,
                        YoutubeId = "hjoMWKaREjk",
                        YoutubeLink = "https://www.youtube.com/watch?v=hjoMWKaREjk&feature=emb_title",
                        YoutubeImage = string.Empty
                    }
                };

                context.VideoHomePages.AddRange(videoHomePages);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedAttachmentTypesAsync(AlbumDbContext context)
        {
            // Seed, if necessary
            if (!context.AttachmentTypes.Any())
            {
                var attachmentTypes = new List<AttachmentType>
                {
                    new AttachmentType
                    {
                        Name = AttachmentTypes.Photo,
                        
                    },
                    new AttachmentType
                    {
                       Name = AttachmentTypes.Video,
                    },
                };

                context.AttachmentTypes.AddRange(attachmentTypes);
                await context.SaveChangesAsync();
            }
        }
    }
}
