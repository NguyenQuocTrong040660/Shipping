using ShippingApp.Domain.Entities;
using ShippingApp.Domain.Interfaces;
using ShippingApp.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingApp.Migration
{
    public interface ISeedShippingApp
    {
        void SeedData();
    }
    public class SeedShippingApp : ISeedShippingApp
    {
        private readonly IShippingAppDbContext _ShippingAppDbContext;
        public SeedShippingApp(IShippingAppDbContext context)
        {
            _ShippingAppDbContext = context;
        }

        public void SeedData()
        {
            Seed_ProductType();
            Seed_Country();
            Seed_Brand();
            Seed_ProductOverview();
        }

        private void Seed_ProductType()
        {
            if (!_ShippingAppDbContext.ProductType.Any())
            {
                List<ProductType> productTypes = new List<ProductType>()
                {
                    new ProductType()
                    {
                        Id = Guid.NewGuid(),
                        ProductTypeName = "Dao - Chip - Mũi Khoan"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Thiệt Bị Đo WENZEL"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Thiệt Bị Giám Sát Lực Kistler"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Thiệt Bị Khí Nén CKD"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Thiệt Bị Đo OHM"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Fanuc"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Hệ Thống Đồ Gá System 3R"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Lò Tôi Nabertherm"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Máy Hàn Khuôn"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Máy Rửa Khuôn"
                    },
                    new ProductType()
                    {
                        Id =  Guid.NewGuid(),
                        ProductTypeName = "Thiệt Bị Đo Renishaw"
                    }
                };
                _ShippingAppDbContext.ProductType.AddRange(productTypes);
            }
            _ShippingAppDbContext.SaveChanges();
        }

        private void Seed_ProductOverview()
        {
            if (!_ShippingAppDbContext.ProductOverview.Any())
            {
                List<ProductType> productTypes = _ShippingAppDbContext.ProductType.ToList();
                List<Brand> brands = _ShippingAppDbContext.Brand.ToList();
                List<Country> countries = _ShippingAppDbContext.Country.ToList();
                List<ProductOverview> productOverviews = new List<ProductOverview>()
                {
                    new ProductOverview()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = "Mũi Taro OSG",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Dao - Chip - Mũi Khoan")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("VN"),
                        //CountryCode = countries.Where(x=>x.CountryCode.Contains("VN")).ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Toshiba")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = "Dao Phay OSG",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Dao - Chip - Mũi Khoan")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("VN"),
                        //CountryCode = _productDbContext.Country.Find("VN").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Toshiba")).FirstOrDefault()

                    },
                    new ProductOverview()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = "Mũi Khoan OSG",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Dao - Chip - Mũi Khoan")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("US"),
                        //CountryCode = _productDbContext.Country.Find("US").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Toshiba")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                         Id = Guid.NewGuid(),
                        ProductName = "Linh kiện Fanuc",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Fanuc")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "HTS phân phối các sản phẩm của FANUC như: robot, robodrill,…. và các phụ kiện máy FANUC như: bộ nguồn, màn hình, controler,….",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("VN"),
                        //CountryCode = _productDbContext.Country.Find("VN").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Fujitsu")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                         Id = Guid.NewGuid(),
                        ProductName = "Precision 2-3/8” Toolmaker Vise Machinist Tookmaking Clamp Vice Replacement Jaw",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Hệ Thống Đồ Gá System 3R")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("AU"),
                        //CountryCode = _productDbContext.Country.Find("AU").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Fujitsu")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                         Id = Guid.NewGuid(),
                        ProductName = "Đồ gá cho máy EDM sản xuất điện cực",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Hệ Thống Đồ Gá System 3R")).FirstOrDefault(),
                        HighLevelDesc = "Kinh nghiệm cho thấy thay vì thay đổi từng bước trong chương trình để rút ngắn thời gian gia công, thì có một cách khác đơn giản hơn nhiều mà tăng năng suất – đó là giảm thời gian dừng máy. Với giải pháp của SYSTEM 3R, việc gá đặt chưa bao giờ đơn giản và nhanh gọn như thế.",
                        MediumLevelDesc = "Lắp các máy có cùng hệ thống tham chiếu có nghĩa là các điện cực và phôi có thể được di chuyển giữa các máy mà không cần căn chỉnh và kiểm tra tiếp theo –  Thiết lập một phút ",
                        NormalLevelDesc = "Mỗi phút có thể được chuyển đổi từ thời gian cài đặt bên trong sang bên ngoài sẽ tăng thời gian trục chính của máy và với năng suất của doanh nghiệp",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("CH"),
                        //CountryCode = _productDbContext.Country.Find("CH").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Yamada")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = "Lò tôi Nabertherm",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Lò Tôi Nabertherm")).FirstOrDefault(),
                        HighLevelDesc = "HTS tự hào là đại lý phân phối các sản phẩm lò tôi của hãng Nabertherm cung cấp cho rất nhiều công ty và dự án ở Việt Nam. Với đội ngũ kỹ thuật mạnh và sự hỗ trợ tuyệt vời của chuyên gia hãng, HTS tự tin về chất lượng sản phẩm cũng như chất lượng dịch vụ, đáp ứng cả những khách hàng với các yêu cầu khó nhất.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("KR"),
                        //CountryCode = _productDbContext.Country.Find("KR").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Yamada")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                         Id = Guid.NewGuid(),
                        ProductName = "Máy mài khuôn",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Máy Hàn Khuôn")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("VN"),
                        //CountryCode = _productDbContext.Country.Find("VN").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Komatsu")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                         Id = Guid.NewGuid(),
                        ProductName = "PowerWELD-Arm200",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Máy Hàn Khuôn")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("JP"),
                        //CountryCode = _productDbContext.Country.Find("SG").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Komatsu")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                         Id = Guid.NewGuid(),
                        ProductName = "Máy Hàn Power Weld AWL",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Máy Hàn Khuôn")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("SG"),
                        //CountryCode = _productDbContext.Country.Find("SG").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Komatsu")).FirstOrDefault()
                    },
                    new ProductOverview()
                    {
                         Id = Guid.NewGuid(),
                        ProductName = "Máy hàn khuôn Alpha Laser",
                        ProductType = (ProductType)productTypes.Where(x => x.ProductTypeName.Contains("Máy Hàn Khuôn")).FirstOrDefault(),
                        HighLevelDesc = "Liên hệ để được tư vấn.",
                        MediumLevelDesc = "Liên hệ để được tư vấn.",
                        NormalLevelDesc = "Liên hệ để được tư vấn.",
                        ImageName = "",
                        ImageUrl  = "",
                        Country = _ShippingAppDbContext.Country.Find("VN"),
                        //CountryCode = _productDbContext.Country.Find("VN").ToString(),
                        Brand = (Brand)brands.Where(x => x.BrandName.Contains("Volvo")).FirstOrDefault()
                    }

                };
                _ShippingAppDbContext.ProductOverview.AddRange(productOverviews);
            }
            _ShippingAppDbContext.SaveChanges();
        }

        private void Seed_Country()
        {
            if (!_ShippingAppDbContext.Country.Any())
            {
                List<Country> countries = new List<Country>()
                {
                    new Country()
{
  CountryCode = "AF",
  CountryName = "Afghanistan"
},
new Country()
{
  CountryCode = "AX",
  CountryName = "Åland Islands"
},
new Country()
{
  CountryCode = "AL",
  CountryName = "Albania"
},
new Country()
{
  CountryCode = "DZ",
  CountryName = "Algeria"
},
new Country()
{
  CountryCode = "AS",
  CountryName = "American Samoa"
},
new Country()
{
  CountryCode = "AD",
  CountryName = "Andorra"
},
new Country()
{
  CountryCode = "AO",
  CountryName = "Angola"
},
new Country()
{
  CountryCode = "AI",
  CountryName = "Anguilla"
},
new Country()
{
  CountryCode = "AQ",
  CountryName = "Antarctica"
},
new Country()
{
  CountryCode = "AG",
  CountryName = "Antigua and Barbuda"
},
new Country()
{
  CountryCode = "AR",
  CountryName = "Argentina"
},
new Country()
{
  CountryCode = "AM",
  CountryName = "Armenia"
},
new Country()
{
  CountryCode = "AW",
  CountryName = "Aruba"
},
new Country()
{
  CountryCode = "AU",
  CountryName = "Australia"
},
new Country()
{
  CountryCode = "AT",
  CountryName = "Austria"
},
new Country()
{
  CountryCode = "AZ",
  CountryName = "Azerbaijan"
},
new Country()
{
  CountryCode = "BS",
  CountryName = "Bahamas"
},
new Country()
{
  CountryCode = "BH",
  CountryName = "Bahrain"
},
new Country()
{
  CountryCode = "BD",
  CountryName = "Bangladesh"
},
new Country()
{
  CountryCode = "BB",
  CountryName = "Barbados"
},
new Country()
{
  CountryCode = "BY",
  CountryName = "Belarus"
},
new Country()
{
  CountryCode = "BE",
  CountryName = "Belgium"
},
new Country()
{
  CountryCode = "BZ",
  CountryName = "Belize"
},
new Country()
{
  CountryCode = "BJ",
  CountryName = "Benin"
},
new Country()
{
  CountryCode = "BM",
  CountryName = "Bermuda"
},
new Country()
{
  CountryCode = "BT",
  CountryName = "Bhutan"
},
new Country()
{
  CountryCode = "BO",
  CountryName = "Bolivia (Plurinational State of)"
},
new Country()
{
  CountryCode = "BQ",
  CountryName = "Bonaire, Sint Eustatius and Saba"
},
new Country()
{
  CountryCode = "BA",
  CountryName = "Bosnia and Herzegovina"
},
new Country()
{
  CountryCode = "BW",
  CountryName = "Botswana"
},
new Country()
{
  CountryCode = "BV",
  CountryName = "Bouvet Island"
},
new Country()
{
  CountryCode = "BR",
  CountryName = "Brazil"
},
new Country()
{
  CountryCode = "IO",
  CountryName = "British Indian Ocean Territory"
},
new Country()
{
  CountryCode = "BN",
  CountryName = "Brunei Darussalam"
},
new Country()
{
  CountryCode = "BG",
  CountryName = "Bulgaria"
},
new Country()
{
  CountryCode = "BF",
  CountryName = "Burkina Faso"
},
new Country()
{
  CountryCode = "BI",
  CountryName = "Burundi"
},
new Country()
{
  CountryCode = "CV",
  CountryName = "Cabo Verde"
},
new Country()
{
  CountryCode = "KH",
  CountryName = "Cambodia"
},
new Country()
{
  CountryCode = "CM",
  CountryName = "Cameroon"
},
new Country()
{
  CountryCode = "CA",
  CountryName = "Canada"
},
new Country()
{
  CountryCode = "KY",
  CountryName = "Cayman Islands"
},
new Country()
{
  CountryCode = "CF",
  CountryName = "Central African Republic"
},
new Country()
{
  CountryCode = "TD",
  CountryName = "Chad"
},
new Country()
{
  CountryCode = "CL",
  CountryName = "Chile"
},
new Country()
{
  CountryCode = "CN",
  CountryName = "China"
},
new Country()
{
  CountryCode = "CX",
  CountryName = "Christmas Island"
},
new Country()
{
  CountryCode = "CC",
  CountryName = "Cocos (Keeling) Islands"
},
new Country()
{
  CountryCode = "CO",
  CountryName = "Colombia"
},
new Country()
{
  CountryCode = "KM",
  CountryName = "Comoros"
},
new Country()
{
  CountryCode = "CG",
  CountryName = "Congo"
},
new Country()
{
  CountryCode = "CD",
  CountryName = "Congo, Democratic Republic of the"
},
new Country()
{
  CountryCode = "CK",
  CountryName = "Cook Islands"
},
new Country()
{
  CountryCode = "CR",
  CountryName = "Costa Rica"
},
new Country()
{
  CountryCode = "CI",
  CountryName = "Côte d'Ivoire"
},
new Country()
{
  CountryCode = "HR",
  CountryName = "Croatia"
},
new Country()
{
  CountryCode = "CU",
  CountryName = "Cuba"
},
new Country()
{
  CountryCode = "CW",
  CountryName = "Curaçao"
},
new Country()
{
  CountryCode = "CY",
  CountryName = "Cyprus"
},
new Country()
{
  CountryCode = "CZ",
  CountryName = "Czechia"
},
new Country()
{
  CountryCode = "DK",
  CountryName = "Denmark"
},
new Country()
{
  CountryCode = "DJ",
  CountryName = "Djibouti"
},
new Country()
{
  CountryCode = "DM",
  CountryName = "Dominica"
},
new Country()
{
  CountryCode = "DO",
  CountryName = "Dominican Republic"
},
new Country()
{
  CountryCode = "EC",
  CountryName = "Ecuador"
},
new Country()
{
  CountryCode = "EG",
  CountryName = "Egypt"
},
new Country()
{
  CountryCode = "SV",
  CountryName = "El Salvador"
},
new Country()
{
  CountryCode = "GQ",
  CountryName = "Equatorial Guinea"
},
new Country()
{
  CountryCode = "ER",
  CountryName = "Eritrea"
},
new Country()
{
  CountryCode = "EE",
  CountryName = "Estonia"
},
new Country()
{
  CountryCode = "SZ",
  CountryName = "Eswatini"
},
new Country()
{
  CountryCode = "ET",
  CountryName = "Ethiopia"
},
new Country()
{
  CountryCode = "FK",
  CountryName = "Falkland Islands (Malvinas)"
},
new Country()
{
  CountryCode = "FO",
  CountryName = "Faroe Islands"
},
new Country()
{
  CountryCode = "FJ",
  CountryName = "Fiji"
},
new Country()
{
  CountryCode = "FI",
  CountryName = "Finland"
},
new Country()
{
  CountryCode = "FR",
  CountryName = "France"
},
new Country()
{
  CountryCode = "GF",
  CountryName = "French Guiana"
},
new Country()
{
  CountryCode = "PF",
  CountryName = "French Polynesia"
},
new Country()
{
  CountryCode = "TF",
  CountryName = "French Southern Territories"
},
new Country()
{
  CountryCode = "GA",
  CountryName = "Gabon"
},
new Country()
{
  CountryCode = "GM",
  CountryName = "Gambia"
},
new Country()
{
  CountryCode = "GE",
  CountryName = "Georgia"
},
new Country()
{
  CountryCode = "DE",
  CountryName = "Germany"
},
new Country()
{
  CountryCode = "GH",
  CountryName = "Ghana"
},
new Country()
{
  CountryCode = "GI",
  CountryName = "Gibraltar"
},
new Country()
{
  CountryCode = "GR",
  CountryName = "Greece"
},
new Country()
{
  CountryCode = "GL",
  CountryName = "Greenland"
},
new Country()
{
  CountryCode = "GD",
  CountryName = "Grenada"
},
new Country()
{
  CountryCode = "GP",
  CountryName = "Guadeloupe"
},
new Country()
{
  CountryCode = "GU",
  CountryName = "Guam"
},
new Country()
{
  CountryCode = "GT",
  CountryName = "Guatemala"
},
new Country()
{
  CountryCode = "GG",
  CountryName = "Guernsey"
},
new Country()
{
  CountryCode = "GN",
  CountryName = "Guinea"
},
new Country()
{
  CountryCode = "GW",
  CountryName = "Guinea-Bissau"
},
new Country()
{
  CountryCode = "GY",
  CountryName = "Guyana"
},
new Country()
{
  CountryCode = "HT",
  CountryName = "Haiti"
},
new Country()
{
  CountryCode = "HM",
  CountryName = "Heard Island and McDonald Islands"
},
new Country()
{
  CountryCode = "VA",
  CountryName = "Holy See"
},
new Country()
{
  CountryCode = "HN",
  CountryName = "Honduras"
},
new Country()
{
  CountryCode = "HK",
  CountryName = "Hong Kong"
},
new Country()
{
  CountryCode = "HU",
  CountryName = "Hungary"
},
new Country()
{
  CountryCode = "IS",
  CountryName = "Iceland"
},
new Country()
{
  CountryCode = "IN",
  CountryName = "India"
},
new Country()
{
  CountryCode = "ID",
  CountryName = "Indonesia"
},
new Country()
{
  CountryCode = "IR",
  CountryName = "Iran (Islamic Republic of)"
},
new Country()
{
  CountryCode = "IQ",
  CountryName = "Iraq"
},
new Country()
{
  CountryCode = "IE",
  CountryName = "Ireland"
},
new Country()
{
  CountryCode = "IM",
  CountryName = "Isle of Man"
},
new Country()
{
  CountryCode = "IL",
  CountryName = "Israel"
},
new Country()
{
  CountryCode = "IT",
  CountryName = "Italy"
},
new Country()
{
  CountryCode = "JM",
  CountryName = "Jamaica"
},
new Country()
{
  CountryCode = "JP",
  CountryName = "Japan"
},
new Country()
{
  CountryCode = "JE",
  CountryName = "Jersey"
},
new Country()
{
  CountryCode = "JO",
  CountryName = "Jordan"
},
new Country()
{
  CountryCode = "KZ",
  CountryName = "Kazakhstan"
},
new Country()
{
  CountryCode = "KE",
  CountryName = "Kenya"
},
new Country()
{
  CountryCode = "KI",
  CountryName = "Kiribati"
},
new Country()
{
  CountryCode = "KP",
  CountryName = "Korea (Democratic People's Republic of)"
},
new Country()
{
  CountryCode = "KR",
  CountryName = "Korea, Republic of"
},
new Country()
{
  CountryCode = "KW",
  CountryName = "Kuwait"
},
new Country()
{
  CountryCode = "KG",
  CountryName = "Kyrgyzstan"
},
new Country()
{
  CountryCode = "LA",
  CountryName = "Lao People's Democratic Republic"
},
new Country()
{
  CountryCode = "LV",
  CountryName = "Latvia"
},
new Country()
{
  CountryCode = "LB",
  CountryName = "Lebanon"
},
new Country()
{
  CountryCode = "LS",
  CountryName = "Lesotho"
},
new Country()
{
  CountryCode = "LR",
  CountryName = "Liberia"
},
new Country()
{
  CountryCode = "LY",
  CountryName = "Libya"
},
new Country()
{
  CountryCode = "LI",
  CountryName = "Liechtenstein"
},
new Country()
{
  CountryCode = "LT",
  CountryName = "Lithuania"
},
new Country()
{
  CountryCode = "LU",
  CountryName = "Luxembourg"
},
new Country()
{
  CountryCode = "MO",
  CountryName = "Macao"
},
new Country()
{
  CountryCode = "MG",
  CountryName = "Madagascar"
},
new Country()
{
  CountryCode = "MW",
  CountryName = "Malawi"
},
new Country()
{
  CountryCode = "MY",
  CountryName = "Malaysia"
},
new Country()
{
  CountryCode = "MV",
  CountryName = "Maldives"
},
new Country()
{
  CountryCode = "ML",
  CountryName = "Mali"
},
new Country()
{
  CountryCode = "MT",
  CountryName = "Malta"
},
new Country()
{
  CountryCode = "MH",
  CountryName = "Marshall Islands"
},
new Country()
{
  CountryCode = "MQ",
  CountryName = "Martinique"
},
new Country()
{
  CountryCode = "MR",
  CountryName = "Mauritania"
},
new Country()
{
  CountryCode = "MU",
  CountryName = "Mauritius"
},
new Country()
{
  CountryCode = "YT",
  CountryName = "Mayotte"
},
new Country()
{
  CountryCode = "MX",
  CountryName = "Mexico"
},
new Country()
{
  CountryCode = "FM",
  CountryName = "Micronesia (Federated States of)"
},
new Country()
{
  CountryCode = "MD",
  CountryName = "Moldova, Republic of"
},
new Country()
{
  CountryCode = "MC",
  CountryName = "Monaco"
},
new Country()
{
  CountryCode = "MN",
  CountryName = "Mongolia"
},
new Country()
{
  CountryCode = "ME",
  CountryName = "Montenegro"
},
new Country()
{
  CountryCode = "MS",
  CountryName = "Montserrat"
},
new Country()
{
  CountryCode = "MA",
  CountryName = "Morocco"
},
new Country()
{
  CountryCode = "MZ",
  CountryName = "Mozambique"
},
new Country()
{
  CountryCode = "MM",
  CountryName = "Myanmar"
},
new Country()
{
  CountryCode = "NA",
  CountryName = "Namibia"
},
new Country()
{
  CountryCode = "NR",
  CountryName = "Nauru"
},
new Country()
{
  CountryCode = "NP",
  CountryName = "Nepal"
},
new Country()
{
  CountryCode = "NL",
  CountryName = "Netherlands"
},
new Country()
{
  CountryCode = "NC",
  CountryName = "New Caledonia"
},
new Country()
{
  CountryCode = "NZ",
  CountryName = "New Zealand"
},
new Country()
{
  CountryCode = "NI",
  CountryName = "Nicaragua"
},
new Country()
{
  CountryCode = "NE",
  CountryName = "Niger"
},
new Country()
{
  CountryCode = "NG",
  CountryName = "Nigeria"
},
new Country()
{
  CountryCode = "NU",
  CountryName = "Niue"
},
new Country()
{
  CountryCode = "NF",
  CountryName = "Norfolk Island"
},
new Country()
{
  CountryCode = "MK",
  CountryName = "North Macedonia"
},
new Country()
{
  CountryCode = "MP",
  CountryName = "Northern Mariana Islands"
},
new Country()
{
  CountryCode = "NO",
  CountryName = "Norway"
},
new Country()
{
  CountryCode = "OM",
  CountryName = "Oman"
},
new Country()
{
  CountryCode = "PK",
  CountryName = "Pakistan"
},
new Country()
{
  CountryCode = "PW",
  CountryName = "Palau"
},
new Country()
{
  CountryCode = "PS",
  CountryName = "Palestine, State of"
},
new Country()
{
  CountryCode = "PA",
  CountryName = "Panama"
},
new Country()
{
  CountryCode = "PG",
  CountryName = "Papua New Guinea"
},
new Country()
{
  CountryCode = "PY",
  CountryName = "Paraguay"
},
new Country()
{
  CountryCode = "PE",
  CountryName = "Peru"
},
new Country()
{
  CountryCode = "PH",
  CountryName = "Philippines"
},
new Country()
{
  CountryCode = "PN",
  CountryName = "Pitcairn"
},
new Country()
{
  CountryCode = "PL",
  CountryName = "Poland"
},
new Country()
{
  CountryCode = "PT",
  CountryName = "Portugal"
},
new Country()
{
  CountryCode = "PR",
  CountryName = "Puerto Rico"
},
new Country()
{
  CountryCode = "QA",
  CountryName = "Qatar"
},
new Country()
{
  CountryCode = "RE",
  CountryName = "Réunion"
},
new Country()
{
  CountryCode = "RO",
  CountryName = "Romania"
},
new Country()
{
  CountryCode = "RU",
  CountryName = "Russian Federation"
},
new Country()
{
  CountryCode = "RW",
  CountryName = "Rwanda"
},
new Country()
{
  CountryCode = "BL",
  CountryName = "Saint Barthélemy"
},
new Country()
{
  CountryCode = "SH",
  CountryName = "Saint Helena, Ascension and Tristan da Cunha"
},
new Country()
{
  CountryCode = "KN",
  CountryName = "Saint Kitts and Nevis"
},
new Country()
{
  CountryCode = "LC",
  CountryName = "Saint Lucia"
},
new Country()
{
  CountryCode = "MF",
  CountryName = "Saint Martin (French part)"
},
new Country()
{
  CountryCode = "PM",
  CountryName = "Saint Pierre and Miquelon"
},
new Country()
{
  CountryCode = "VC",
  CountryName = "Saint Vincent and the Grenadines"
},
new Country()
{
  CountryCode = "WS",
  CountryName = "Samoa"
},
new Country()
{
  CountryCode = "SM",
  CountryName = "San Marino"
},
new Country()
{
  CountryCode = "ST",
  CountryName = "Sao Tome and Principe"
},
new Country()
{
  CountryCode = "SA",
  CountryName = "Saudi Arabia"
},
new Country()
{
  CountryCode = "SN",
  CountryName = "Senegal"
},
new Country()
{
  CountryCode = "RS",
  CountryName = "Serbia"
},
new Country()
{
  CountryCode = "SC",
  CountryName = "Seychelles"
},
new Country()
{
  CountryCode = "SL",
  CountryName = "Sierra Leone"
},
new Country()
{
  CountryCode = "SG",
  CountryName = "Singapore"
},
new Country()
{
  CountryCode = "SX",
  CountryName = "Sint Maarten (Dutch part)"
},
new Country()
{
  CountryCode = "SK",
  CountryName = "Slovakia"
},
new Country()
{
  CountryCode = "SI",
  CountryName = "Slovenia"
},
new Country()
{
  CountryCode = "SB",
  CountryName = "Solomon Islands"
},
new Country()
{
  CountryCode = "SO",
  CountryName = "Somalia"
},
new Country()
{
  CountryCode = "ZA",
  CountryName = "South Africa"
},
new Country()
{
  CountryCode = "GS",
  CountryName = "South Georgia and the South Sandwich Islands"
},
new Country()
{
  CountryCode = "SS",
  CountryName = "South Sudan"
},
new Country()
{
  CountryCode = "ES",
  CountryName = "Spain"
},
new Country()
{
  CountryCode = "LK",
  CountryName = "Sri Lanka"
},
new Country()
{
  CountryCode = "SD",
  CountryName = "Sudan"
},
new Country()
{
  CountryCode = "SR",
  CountryName = "Suriname"
},
new Country()
{
  CountryCode = "SJ",
  CountryName = "Svalbard and Jan Mayen"
},
new Country()
{
  CountryCode = "SE",
  CountryName = "Sweden"
},
new Country()
{
  CountryCode = "CH",
  CountryName = "Switzerland"
},
new Country()
{
  CountryCode = "SY",
  CountryName = "Syrian Arab Republic"
},
new Country()
{
  CountryCode = "TW",
  CountryName = "Taiwan, Province of China"
},
new Country()
{
  CountryCode = "TJ",
  CountryName = "Tajikistan"
},
new Country()
{
  CountryCode = "TZ",
  CountryName = "Tanzania, United Republic of"
},
new Country()
{
  CountryCode = "TH",
  CountryName = "Thailand"
},
new Country()
{
  CountryCode = "TL",
  CountryName = "Timor-Leste"
},
new Country()
{
  CountryCode = "TG",
  CountryName = "Togo"
},
new Country()
{
  CountryCode = "TK",
  CountryName = "Tokelau"
},
new Country()
{
  CountryCode = "TO",
  CountryName = "Tonga"
},
new Country()
{
  CountryCode = "TT",
  CountryName = "Trinidad and Tobago"
},
new Country()
{
  CountryCode = "TN",
  CountryName = "Tunisia"
},
new Country()
{
  CountryCode = "TR",
  CountryName = "Turkey"
},
new Country()
{
  CountryCode = "TM",
  CountryName = "Turkmenistan"
},
new Country()
{
  CountryCode = "TC",
  CountryName = "Turks and Caicos Islands"
},
new Country()
{
  CountryCode = "TV",
  CountryName = "Tuvalu"
},
new Country()
{
  CountryCode = "UG",
  CountryName = "Uganda"
},
new Country()
{
  CountryCode = "UA",
  CountryName = "Ukraine"
},
new Country()
{
  CountryCode = "AE",
  CountryName = "United Arab Emirates"
},
new Country()
{
  CountryCode = "GB",
  CountryName = "United Kingdom of Great Britain and Northern Ireland"
},
new Country()
{
  CountryCode = "US",
  CountryName = "United States of America"
},
new Country()
{
  CountryCode = "UM",
  CountryName = "United States Minor Outlying Islands"
},
new Country()
{
  CountryCode = "UY",
  CountryName = "Uruguay"
},
new Country()
{
  CountryCode = "UZ",
  CountryName = "Uzbekistan"
},
new Country()
{
  CountryCode = "VU",
  CountryName = "Vanuatu"
},
new Country()
{
  CountryCode = "VE",
  CountryName = "Venezuela (Bolivarian Republic of)"
},
new Country()
{
  CountryCode = "VN",
  CountryName = "Viet Nam"
},
new Country()
{
  CountryCode = "VG",
  CountryName = "Virgin Islands (British)"
},
new Country()
{
  CountryCode = "VI",
  CountryName = "Virgin Islands (U.S.)"
},
new Country()
{
  CountryCode = "WF",
  CountryName = "Wallis and Futuna"
},
new Country()
{
  CountryCode = "EH",
  CountryName = "Western Sahara"
},
new Country()
{
  CountryCode = "YE",
  CountryName = "Yemen"
},
new Country()
{
  CountryCode = "ZM",
  CountryName = "Zambia"
},
new Country()
{
  CountryCode = "ZW",
  CountryName = "Zimbabwe"
}

                };
                _ShippingAppDbContext.Country.AddRange(countries);
            }
            _ShippingAppDbContext.SaveChanges();
        }

        private void Seed_Brand()
        {
            if (!_ShippingAppDbContext.Brand.Any())
            {
                List<Brand> brands = new List<Brand>()
                {
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Toshiba"
                    },
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Fujitsu"
                    },
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Yamada"
                    },
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Caterpillar"
                    },
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Komatsu"
                    },
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Doosan Infracore"
                    },
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Volvo"
                    },
                    new Brand()
                    {
                        Id = Guid.NewGuid(),
                        BrandName = "Hitachi"
                    },
                };
                _ShippingAppDbContext.Brand.AddRange(brands);
            }
            _ShippingAppDbContext.SaveChanges();
        }
    }
}
