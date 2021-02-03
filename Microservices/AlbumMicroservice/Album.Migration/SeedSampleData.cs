﻿using Album.Domain.Entities;
using Album.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Album.Migration
{
    public interface ISeedSampleData
    {
        Task<int> SeedData();
        Task<int> SeedNewsData();
    }

    public class SeedSampleData: ISeedSampleData
    {
        private readonly IAlbumDbContext _context;

        public SeedSampleData (IAlbumDbContext context)
        {
            _context = context;
        }

        public async Task<int> SeedData()
        {
            if (!_context.NewsCategories.Any())
            {
                List<NewsCategory> categories = new List<NewsCategory>()
                {
                    new NewsCategory()
                    {
                        CategoryName = "Category1",
                    },
                    new NewsCategory()
                    {
                       CategoryName = "Category2",
                    }
                };

                _context.NewsCategories.AddRange(categories);
            }
            return await _context.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> SeedNewsData()
        {
            if (!_context.News.Any())
            {
                List<Domain.Entities.News> news = new List<Domain.Entities.News>();

                var category = _context.NewsCategories.FirstOrDefault();

                if (category == null) 
                    return 0;

                Random r = new Random();

                for (int i = 0; i < 15; i++)
                {
                    news.Add(new Domain.Entities.News
                    {
                        CategoryId = category.Id,
                        Content = @"<p>
<strong>Quản lý rủi ro và giảm nhẹ</strong>
<br/>
Quản lý và giảm thiểu rủi ro là việc xác định, đánh giá và ưu tiên các rủi ro. Theo định nghĩa của AS9100 Rev. C, rủi ro là tác động của sự không chắc chắn, cho dù tích cực hay tiêu cực khi đạt được các mục tiêu . Rủi ro gây ra sự không chắc chắn có thể xuất phát từ thất bại của dự án (ở bất kỳ giai đoạn nào trong thiết kế, phát triển, sản xuất hoặc duy trì vòng đời), nợ pháp lý, thị trường tài chính và rủi ro tín dụng, cũng như tai nạn và nguyên nhân tự nhiên. Các chiến lược để giảm thiểu rủi ro thường bao gồm giảm nguy cơ rủi ro, giảm tác động tiêu cực nếu rủi ro xảy ra, chuyển rủi ro sang một bên khác, tránh rủi ro hoặc thậm chí chấp nhận một số hoặc tất cả các hậu quả tiềm ẩn hoặc thực tế của một rủi ro cụ thể.
<br/>
Khái niệm rủi ro là tinh tế. Gary Smith, chủ tịch, Gear Manufacturing, Inc., nhận xét, “Giảm thiểu rủi ro bao gồm những thứ như có kế hoạch thảm họa tại chỗ để giải thích các sự kiện bao gồm cả nhân viên chủ chốt bước ra khỏi cửa để bị một chiếc xe tải đâm. Rủi ro có thể vốn có trong các mệnh đề chất lượng PO có chứa một số vùng màu xám có thể chưa được hiểu đầy đủ; nó cũng có thể nằm trong dung sai chặt chẽ hoặc các thông số kỹ thuật khác có thể khó xác thực. ”
<br/>
Trong các điều khoản quy trình sản xuất, một khía cạnh của giảm thiểu rủi ro là phải chắc chắn rằng các sửa đổi chính xác của các tài liệu kỹ thuật, hướng dẫn và thông số kỹ thuật được sử dụng. Và khi dụng cụ và các thiết bị khác (kể cả máy CNC) được sử dụng, điều quan trọng là phải chứng minh tính toàn vẹn của thiết bị liên quan đến sự phù hợp và mục đích tập thể dục của các vật phẩm được sản xuất.
<br/>
Việc sản xuất một sản phẩm phức tạp như một chiếc máy bay hoặc phương tiện không gian đòi hỏi sự chú ý gần như ở mọi bước của quy trình cho mọi bộ phận được sản xuất – dù nhỏ đến mức nào.
<br/>
Đối với GMI, một lĩnh vực quan trọng trong quản lý rủi ro và giảm thiểu là đảm bảo rằng các bộ phận mà nhà sản xuất đáp ứng được dung sai chiều được chỉ định.
</p>",
                        Created = DateTime.Now,
                        ImageName = string.Empty,
                        ImageUrl = string.Empty,
                        Keyword = "bai-viet-so-1",
                        LastModified = DateTime.Now,
                        Descriptions = "Các phép đo chiều chính xác cực kỳ quan trọng để giảm thiểu rủi ro theo AS9100 Rev. C.",
                        Title = "XÁC NHẬN DUNG SAI",
                        Views = r.Next(100, 1000),
                    });
                }

                _context.News.AddRange(news);
            }
            return await _context.SaveChangesAsync(new CancellationToken());
        }
    }
}
