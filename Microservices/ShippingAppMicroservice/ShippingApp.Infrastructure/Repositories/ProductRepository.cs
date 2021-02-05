using ShippingApp.Domain.Interfaces;
using Models = ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Application.Common.Exceptions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Net;
using System.Net.Mail;

namespace ShippingApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IShippingAppDbContext _productDbContext;
        private readonly IMapper _mapper;

        public ProductRepository(IShippingAppDbContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<Models.ProductType> GetAllCategory()
        {
            List<Models.ProductType> results = new List<Models.ProductType>();
            var categories = _productDbContext.ProductType.ToList();

            foreach (var item in categories)
            {
                var model = _mapper.Map<Models.ProductType>(item);
                results.Add(model);
            }

            return results;
        }

        public List<Models.ProductModel> GetAllProducts()
        {
            var products = _productDbContext.Product
                    .AsNoTracking()
                    .ToList();

            var results = _mapper.Map<List<Models.ProductModel>>(products);

            return results.ToList();
        }


        public List<Models.ProductOverview> GetAllProductsHightLight()
        {
            List<Models.ProductOverview> results = new List<Models.ProductOverview>();

            var products = _productDbContext.ProductOverview.Where(x => x.HightlightProduct == true)
                .Include(item => item.ProductType)
                .Include(item => item.Country)
                .Include(item => item.Brand)
                .AsNoTracking()
                .ToList();

            results = _mapper.Map<List<Models.ProductOverview>>(products);
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    if (item.CompanyIndex == 0)
                    {
                        item.CompanyName = "All";
                    }
                    else if (item.CompanyIndex == 1)
                    {
                        item.CompanyName = "Slinks";
                    }
                    else if (item.CompanyIndex == 2)
                    {
                        item.CompanyName = "HTS";
                    }
                    else if (item.CompanyIndex == 3)
                    {
                        item.CompanyName = "Kaizen";
                    }
                    else if (item.CompanyIndex == 4)
                    {
                        item.CompanyName = "Slinks Automation";
                    }
                    else
                    {
                        item.CompanyName = "No Name";
                    }
                }
            }

            return results;
        }


        public  List<Models.ProductType> GetAllProductType(int CompanyIndex)
        {
            //
            List<Models.ProductType> results = new List<Models.ProductType>();
            //var productTypes = _productDbContext.ProductType.ToList();

            if (CompanyIndex == 0)
            {
                var productTypes = _productDbContext.ProductType.ToList();
                foreach (var item in productTypes)
                {
                    var model = _mapper.Map<Models.ProductType>(item);
                    results.Add(model);
                }
            }
            else
            {
                var productTypes = _productDbContext.ProductType
                    .Where(item => item.CompanyIndex == CompanyIndex)
                    .ToList();
                foreach (var item in productTypes)
                {
                    var model = _mapper.Map<Models.ProductType>(item);
                    results.Add(model);
                }
            }

            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    if (item.CompanyIndex == 0)
                    {
                        item.CompanyName = "All";
                    }
                    else if (item.CompanyIndex == 1)
                    {
                        item.CompanyName = "Slinks";
                    }
                    else if (item.CompanyIndex == 2)
                    {
                        item.CompanyName = "HTS";
                    }
                    else if (item.CompanyIndex == 3)
                    {
                        item.CompanyName = "Kaizen";
                    }
                    else if (item.CompanyIndex == 4)
                    {
                        item.CompanyName = "Slinks Automation";
                    }
                    else
                    {
                        item.CompanyName = "No Name";
                    }
                }
            }
            return results;
        }

        public Models.ProductGroup GetAllProductGroup()
        {
            Models.ProductGroup results = new Models.ProductGroup();
            var countries = _productDbContext.Country.ToList();
            var brands = _productDbContext.Brand.ToList();

            foreach(var item in countries)
            {
                var model = _mapper.Map<Models.Country>(item);
                results.Countries.Add(model);
            }

            foreach(var item in brands)
            {
                var model = _mapper.Map<Models.Brand>(item);
                results.Brands.Add(model);
            }

            return results;
        }

        public async Task<Models.ProductModel> GetProductsbyID(Guid Id)
        {
            var result = await _productDbContext.Product.FindAsync(Id);

            return _mapper.Map<Models.ProductModel>(result);
        }

        public List<Models.Country> GetProductCountry()
        {
            List<Models.Country> results = new List<Models.Country>();
            var productCountries = _productDbContext.Country.ToList();

            foreach (var item in productCountries)
            {
                var model = _mapper.Map<Models.Country>(item);
                results.Add(model);
            }

            return results;
        }

        public List<Models.Brand> GetProductBrand()
        {
            List<Models.Brand> results = new List<Models.Brand>();
            var productBrand = _productDbContext.Brand.ToList();

            foreach (var item in productBrand)
            {
                var model = _mapper.Map<Models.Brand>(item);
                results.Add(model);
            }

            return results;
        }

        public List<Models.ProductOverview> GetProductByCountries(string Id)
        {
            List<Models.ProductOverview> results = new List<Models.ProductOverview>();
            var productOverview = _productDbContext.ProductOverview.Find(Id);

            //foreach (var item in productBrand)
            //{
            //    var model = _mapper.Map<Models.Brand>(item);
            //    results.Add(model);
            //}

            return results;
        }

        public List<Models.ProductOverview> GetProductByBrand(string BrandCode)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateProductType(Models.ProductType entity)
        {
            var result = await _productDbContext.ProductType.FindAsync(entity.Id);
            var productTypeQuery = _productDbContext.ProductType.Where(s => s.ProductTypeName.Trim().Replace(" ","") == entity.ProductTypeName.Trim().Replace(" ","")).FirstOrDefault();

            //if (result != null)
            //{
            //     return await UpdateProductType(entity);
            //}

            if (productTypeQuery != null)
            {
                return -1;
               
            }

            _productDbContext.ProductType.Add(new Entities.ProductType
            {
                Id = entity.Id,
                ProductTypeName = entity.ProductTypeName,
                CompanyIndex = entity.CompanyIndex
            });

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<Entities.ProductType> GetProductTypeByName(string ProductTypeName)
        {
            var result =  _productDbContext.ProductType.Where(s => s.ProductTypeName == ProductTypeName).FirstOrDefault();
            return result;
        }

        public async Task<Entities.ProductType> GetProductTypeByCode(Guid ProductTypeCode)
        {
            var result = _productDbContext.ProductType.FindAsync(ProductTypeCode);
            return await result;
        }

        public async Task<int> DeleteProductType(Guid ProductTypeCode)
        {
            var entity = await _productDbContext.ProductType.FindAsync(ProductTypeCode);

            if (entity == null)
            {
                return default;
            }

            _productDbContext.ProductType.Remove(entity);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> UpdateProductType(Models.ProductType entity)
        {
            var result = await _productDbContext.ProductType.FindAsync(entity.Id);

            if (result == null)
            {
                return default;
            }

            result.ProductTypeName = entity.ProductTypeName;
            result.CompanyIndex = entity.CompanyIndex;

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> CreateProductOverView(Models.ProductModel productModel)
        {
            var productEntity = _mapper.Map<Entities.ProductEntity>(productModel);

            var result = await _productDbContext.Product.FindAsync(productEntity.Id);

            if (result != null)
            {
                return 0;
            }

            _productDbContext.Product.Add(productEntity);
            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> DeleteProductOverView(Guid Id)
        {
            var entity = await _productDbContext.Product.FindAsync(Id);

            if (entity == null)
            {
                return default;
            }

            _productDbContext.Product.Remove(entity);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> UpdateProductOverView(Entities.ProductOverview entity)
        {
            var result = await _productDbContext.ProductOverview.FindAsync(entity.Id);
            if (result == null)
            {
                return default;
            }

            result.ProductName = entity.ProductName;
            result.HighLevelDesc = entity.HighLevelDesc;
            result.MediumLevelDesc = entity.MediumLevelDesc;
            result.NormalLevelDesc = entity.NormalLevelDesc;
            result.ImageName = entity.ImageName;
            result.ImageUrl = entity.ImageUrl;
            result.CompanyIndex = entity.CompanyIndex;
            result.HightlightProduct = entity.HightlightProduct;
            result.ProductTypeId = entity.ProductTypeId;
            result.BrandId = entity.BrandId;
            result.Country = _productDbContext.Country.Find(entity.CountryCode);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }


        public async Task<int> CreateCountry(Models.Country entity)
        {
            var result = await _productDbContext.Country.FindAsync(entity.CountryCode);
            var countryQuery = _productDbContext.Country.Where(s => s.CountryName.Trim().Replace(" ", "") == entity.CountryName.Trim().Replace(" ", "")).FirstOrDefault();

            if (result != null)
            {
                //return await UpdateProductType(entity);

            }

            if (countryQuery != null)
            {
                return -1;

            }

            _productDbContext.Country.Add(new Entities.Country
            {
                CountryCode = entity.CountryCode,
                CountryName = entity.CountryName
            });



            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }


        public List<Models.Country> GetAllCountry()
        {
            List<Models.Country> results = new List<Models.Country>();
            var countries = _productDbContext.Country.ToList();

            foreach (var item in countries)
            {
                var model = _mapper.Map<Models.Country>(item);
                results.Add(model);
            }

            return results;
        }


        public async Task<Entities.Country> GetCountryByName(string CountryName)
        {
            var result = _productDbContext.Country.Where(s => s.CountryName == CountryName).FirstOrDefault();
            return result;
        }

        public async Task<Entities.Country> GetCountryByCode(string countryCode)
        {
            var result = _productDbContext.Country.FindAsync(countryCode);
            return await result;
        }

        //public Task<int> UpdateCountry(Country entity)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<int> UpdateCountry(string CountryCode, Models.Country entity)
        {
            var result = await _productDbContext.Country.FindAsync(CountryCode);

            if (result == null)
            {
                return default;
            }
            //result.CountryCode = entity.CountryCode;
            result.CountryName = entity.CountryName;

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<Entities.Brand> GetBrandByCode(Guid Id)
        {
            var result = _productDbContext.Brand.FindAsync(Id);
            return await result;
        }

        public async Task<int> CreateBrand(Models.Brand entity)
        {
            var result = await _productDbContext.Brand.FindAsync(entity.Id);
            var productTypeQuery = _productDbContext.Brand.Where(s => s.BrandName.Trim().Replace(" ", "") == entity.BrandName.Trim().Replace(" ", "")).FirstOrDefault();

            //if (result != null)
            //{
            //    return await UpdateBrand(entity);
            //}

            if (productTypeQuery != null)
            {
                return -1;

            }

            _productDbContext.Brand.Add(new Entities.Brand
            {
                Id = entity.Id,
                BrandName = entity.BrandName,
                CompanyIndex = entity.CompanyIndex
            });

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> UpdateBrand(Models.Brand entity)
        {
            var result = await _productDbContext.Brand.FindAsync(entity.Id);

            if (result == null)
            {
                return default;
            }

            result.BrandName = entity.BrandName;
            result.CompanyIndex = entity.CompanyIndex;

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public List<Models.Brand> GetAllBrand(int CompanyIndex)
        {
            List<Models.Brand> results = new List<Models.Brand>();
            

            if (CompanyIndex == 0)
            {
                var brands = _productDbContext.Brand.ToList();
                foreach (var item in brands)
                {
                    var model = _mapper.Map<Models.Brand>(item);
                    results.Add(model);
                }
            }
            else
            {
                var brands = _productDbContext.Brand
                    .Where(item => item.CompanyIndex == CompanyIndex)
                    .ToList();
                foreach (var item in brands)
                {
                    var model = _mapper.Map<Models.Brand>(item);
                    results.Add(model);
                }
            }

            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    if (item.CompanyIndex == 0)
                    {
                        item.CompanyName = "All";
                    }
                    else if (item.CompanyIndex == 1)
                    {
                        item.CompanyName = "Slinks";
                    }
                    else if (item.CompanyIndex == 2)
                    {
                        item.CompanyName = "HTS";
                    }
                    else if (item.CompanyIndex == 3)
                    {
                        item.CompanyName = "Kaizen";
                    }
                    else if (item.CompanyIndex == 4)
                    {
                        item.CompanyName = "Slinks Automation";
                    }
                    else
                    {
                        item.CompanyName = "No Name";
                    }
                }
            }


            return results;
        }

        public async Task<int> DeleteBrand(Guid Id)
        {
            var entity = await _productDbContext.Brand.FindAsync(Id);

            if (entity == null)
            {
                return default;
            }

            _productDbContext.Brand.Remove(entity);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }


        public async Task<int> DeleteCountry(string CountryCode)
        {
            var entity = await _productDbContext.Country.FindAsync(CountryCode);

            if (entity == null)
            {
                return default;
            }
            

            _productDbContext.Country.Remove(entity);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<Entities.Customer> CreatCustomer(Models.Customer customer)
        {
            //_productDbContext.Customers.Add(new Entities.Customer
            //{
            //    Id = new Guid(),
            //    Address1 = customer.Address1,
            //    Address2 = customer.Address2,
            //    PhoneNumber = customer.PhoneNumber,
            //    Email = customer.PhoneNumber,
            //    CompanyName = customer.CompanyName,
            //    CustomerName = customer.CustomerName,
            //    Note = customer.Note
            //});

            Entities.Customer newCustomer = new Entities.Customer();
            newCustomer.Id = new Guid();
            newCustomer.Address1 = customer.Address1;
            newCustomer.Address2 = customer.Address2;
            newCustomer.PhoneNumber = customer.PhoneNumber;
            newCustomer.Email = customer.PhoneNumber;
            newCustomer.CompanyName = customer.CompanyName;
            newCustomer.CustomerName = customer.CustomerName;
            newCustomer.Note = customer.Note;
            _productDbContext.Customers.Add(newCustomer);

            await _productDbContext.SaveChangesAsync(new CancellationToken());

            return newCustomer;

        }

        public void SendEmail(Models.Order order)
        {
            string address = "an.th@grex-solutions.com";
            string subject = "Mail Tư Vấn Sản Phẩm";
            string cart = "";
            string email = "tangan2215@gmail.com";
            string password = "antrinh2315";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            if (order.OrderDetails.Count > 0)
            {
                foreach (var item in order.OrderDetails)
                {
                    cart += "<br/>" + "Tên sản phẩm: " + item.ProductName;
                }
            }


            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = string.Format("Bạn vừa nhận được liên hê từ: <b style='color:red'>{0}</b><br/>SĐT: {1}<br/>Email: {2}<br/>Nội dung: yêu cầu tư vấn sản phẩm  </br>" + cart, order.Customer.CustomerName,order.Customer.PhoneNumber,order.Customer.Email);
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }


        public async Task<int> CreatOder(Models.Order order)
        {

            //SendEmail(order);

            var customer = await CreatCustomer(order.Customer);
            Entities.Order orderEntity = new Entities.Order();
            orderEntity.Id = new Guid();
            orderEntity.CustomerId = customer.Id;

            _productDbContext.Orders.Add(orderEntity);

            await _productDbContext.SaveChangesAsync(new CancellationToken());

            List<Entities.OrderDetail> listOrderDetail = new List<Entities.OrderDetail>();
            foreach (var orderDetail in order.OrderDetails)
            {
                Entities.OrderDetail orderDetailEntity = new Entities.OrderDetail();
                orderDetailEntity.OrderId = new Guid();
                orderDetailEntity.ProductId = orderDetail.ProductId;
                orderDetailEntity.ProductName = orderDetail.ProductName;
                orderDetailEntity.Quantity = orderDetail.Quantity;
                orderDetailEntity.CartId = orderEntity.Id;

                //_productDbContext.orderDetails.Add(orderDetailEntity);
                
                listOrderDetail.Add(orderDetailEntity);
            }

            _productDbContext.orderDetails.AddRange(listOrderDetail);
            await _productDbContext.SaveChangesAsync(new CancellationToken());
            return 1;
        }
    }
}

