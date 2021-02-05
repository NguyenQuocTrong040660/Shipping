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
    public class ShippingAppRepository : IShippingAppRepository
    {
        private readonly IShippingAppDbContext _shippingAppDbContext;
        private readonly IMapper _mapper;

        public ShippingAppRepository(IShippingAppDbContext productDbContext, IMapper mapper)
        {
            _shippingAppDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<Models.ProductType> GetAllCategory()
        {
            List<Models.ProductType> results = new List<Models.ProductType>();
            var categories = _shippingAppDbContext.ProductType.ToList();

            foreach (var item in categories)
            {
                var model = _mapper.Map<Models.ProductType>(item);
                results.Add(model);
            }

            return results;
        }

        public List<Models.ProductModel> GetAllProducts()
        {
            var products = _shippingAppDbContext.Product
                    .AsNoTracking()
                    .ToList();

            var results = _mapper.Map<List<Models.ProductModel>>(products);

            return results.ToList();
        }

        public  List<Models.ProductType> GetAllProductType(int CompanyIndex)
        {
            //
            List<Models.ProductType> results = new List<Models.ProductType>();
            //var productTypes = _productDbContext.ProductType.ToList();

            if (CompanyIndex == 0)
            {
                var productTypes = _shippingAppDbContext.ProductType.ToList();
                foreach (var item in productTypes)
                {
                    var model = _mapper.Map<Models.ProductType>(item);
                    results.Add(model);
                }
            }
            else
            {
                var productTypes = _shippingAppDbContext.ProductType
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

        public async Task<Models.ProductModel> GetProductsbyID(Guid Id)
        {
            var result = await _shippingAppDbContext.Product.FindAsync(Id);

            return _mapper.Map<Models.ProductModel>(result);
        }

        public List<Models.Country> GetProductCountry()
        {
            List<Models.Country> results = new List<Models.Country>();
            var productCountries = _shippingAppDbContext.Country.ToList();

            foreach (var item in productCountries)
            {
                var model = _mapper.Map<Models.Country>(item);
                results.Add(model);
            }

            return results;
        }

       
        public async Task<int> CreateProductType(Models.ProductType entity)
        {
            var result = await _shippingAppDbContext.ProductType.FindAsync(entity.Id);
            var productTypeQuery = _shippingAppDbContext.ProductType.Where(s => s.ProductTypeName.Trim().Replace(" ","") == entity.ProductTypeName.Trim().Replace(" ","")).FirstOrDefault();

            //if (result != null)
            //{
            //     return await UpdateProductType(entity);
            //}

            if (productTypeQuery != null)
            {
                return -1;
               
            }

            _shippingAppDbContext.ProductType.Add(new Entities.ProductType
            {
                Id = entity.Id,
                ProductTypeName = entity.ProductTypeName,
                CompanyIndex = entity.CompanyIndex
            });

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<Entities.ProductType> GetProductTypeByName(string ProductTypeName)
        {
            var result =  _shippingAppDbContext.ProductType.Where(s => s.ProductTypeName == ProductTypeName).FirstOrDefault();
            return result;
        }

        public async Task<Entities.ProductType> GetProductTypeByCode(Guid ProductTypeCode)
        {
            var result = _shippingAppDbContext.ProductType.FindAsync(ProductTypeCode);
            return await result;
        }

        public async Task<int> DeleteProductType(Guid ProductTypeCode)
        {
            var entity = await _shippingAppDbContext.ProductType.FindAsync(ProductTypeCode);

            if (entity == null)
            {
                return default;
            }

            _shippingAppDbContext.ProductType.Remove(entity);

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> UpdateProductType(Models.ProductType entity)
        {
            var result = await _shippingAppDbContext.ProductType.FindAsync(entity.Id);

            if (result == null)
            {
                return default;
            }

            result.ProductTypeName = entity.ProductTypeName;
            result.CompanyIndex = entity.CompanyIndex;

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> CreateNewProduct(Models.ProductModel productModel)
        {
            var productEntity = _mapper.Map<Entities.ProductEntity>(productModel);

            var result = await _shippingAppDbContext.Product.FindAsync(productEntity.Id);

            if (result != null)
            {
                return 0;
            }

            _shippingAppDbContext.Product.Add(productEntity);
            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> DeleteProductByID(Guid Id)
        {
            var entity = await _shippingAppDbContext.Product.FindAsync(Id);

            if (entity == null)
            {
                return default;
            }

            _shippingAppDbContext.Product.Remove(entity);

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> UpdateProduct(Models.ProductModel productModel)
        {
            var result = await _shippingAppDbContext.Product.FindAsync(productModel.Id);

            if (result == null)
            {
                return default;
            }

            _mapper.Map(productModel, result);
            //_shippingAppDbContext.Product.Update(productEntity);

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }


        public async Task<int> CreateCountry(Models.Country entity)
        {
            var result = await _shippingAppDbContext.Country.FindAsync(entity.CountryCode);
            var countryQuery = _shippingAppDbContext.Country.Where(s => s.CountryName.Trim().Replace(" ", "") == entity.CountryName.Trim().Replace(" ", "")).FirstOrDefault();

            if (result != null)
            {
                //return await UpdateProductType(entity);

            }

            if (countryQuery != null)
            {
                return -1;

            }

            _shippingAppDbContext.Country.Add(new Entities.Country
            {
                CountryCode = entity.CountryCode,
                CountryName = entity.CountryName
            });



            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }


        public List<Models.Country> GetAllCountry()
        {
            List<Models.Country> results = new List<Models.Country>();
            var countries = _shippingAppDbContext.Country.ToList();

            foreach (var item in countries)
            {
                var model = _mapper.Map<Models.Country>(item);
                results.Add(model);
            }

            return results;
        }


        public async Task<Entities.Country> GetCountryByName(string CountryName)
        {
            var result = _shippingAppDbContext.Country.Where(s => s.CountryName == CountryName).FirstOrDefault();
            return result;
        }

        public async Task<Entities.Country> GetCountryByCode(string countryCode)
        {
            var result = _shippingAppDbContext.Country.FindAsync(countryCode);
            return await result;
        }

        //public Task<int> UpdateCountry(Country entity)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<int> UpdateCountry(string CountryCode, Models.Country entity)
        {
            var result = await _shippingAppDbContext.Country.FindAsync(CountryCode);

            if (result == null)
            {
                return default;
            }
            //result.CountryCode = entity.CountryCode;
            result.CountryName = entity.CountryName;

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        

        public async Task<int> DeleteCountry(string CountryCode)
        {
            var entity = await _shippingAppDbContext.Country.FindAsync(CountryCode);

            if (entity == null)
            {
                return default;
            }
            

            _shippingAppDbContext.Country.Remove(entity);

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        //public void SendEmail(Models.Order order)
        //{
        //    string address = "an.th@grex-solutions.com";
        //    string subject = "Mail Tư Vấn Sản Phẩm";
        //    string cart = "";
        //    string email = "tangan2215@gmail.com";
        //    string password = "antrinh2315";

        //    var loginInfo = new NetworkCredential(email, password);
        //    var msg = new MailMessage();
        //    var smtpClient = new SmtpClient("smtp.gmail.com", 587);

        //    if (order.OrderDetails.Count > 0)
        //    {
        //        foreach (var item in order.OrderDetails)
        //        {
        //            cart += "<br/>" + "Tên sản phẩm: " + item.ProductName;
        //        }
        //    }


        //    msg.From = new MailAddress(email);
        //    msg.To.Add(new MailAddress(address));
        //    msg.Subject = subject;
        //    msg.Body = string.Format("Bạn vừa nhận được liên hê từ: <b style='color:red'>{0}</b><br/>SĐT: {1}<br/>Email: {2}<br/>Nội dung: yêu cầu tư vấn sản phẩm  </br>" + cart, order.Customer.CustomerName,order.Customer.PhoneNumber,order.Customer.Email);
        //    msg.IsBodyHtml = true;

        //    smtpClient.EnableSsl = true;
        //    smtpClient.UseDefaultCredentials = false;
        //    smtpClient.Credentials = loginInfo;
        //    smtpClient.Send(msg);
        //}

        
    }
}

