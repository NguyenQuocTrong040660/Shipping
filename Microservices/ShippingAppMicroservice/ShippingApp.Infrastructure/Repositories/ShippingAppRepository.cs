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

        public List<Models.Product> GetAllProducts()
        {
            var products = _shippingAppDbContext.Product
                    .AsNoTracking()
                    .ToList();

            var results = _mapper.Map<List<Models.Product>>(products);

            return results.ToList();
        }

        public async Task<Models.Product> GetProductsByID(Guid Id)
        {
            var result = await _shippingAppDbContext.Product.FindAsync(Id);

            return _mapper.Map<Models.Product>(result);
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
       
        public async Task<int> CreateNewProduct(Models.Product productModel)
        {
            var productEntity = _mapper.Map<Entities.Product>(productModel);

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
            var product = await _shippingAppDbContext.Product.FindAsync(Id);

            if (product == null)
            {
                return default;
            }

            _shippingAppDbContext.Product.Remove(product);

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> UpdateProduct(Models.Product productModel)
        {
            var result = await _shippingAppDbContext.Product.FindAsync(productModel.Id);

            if (result == null)
            {
                return default;
            }

            _mapper.Map(productModel, result);

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

        public async Task<int> CreateNewShippingPlan(Models.ShippingPlan shippingPlan)
        {
            var shippingPlanEntity = _mapper.Map<Entities.ShippingPlan>(shippingPlan);

            var result = await _shippingAppDbContext.ShippingPlan.FindAsync(shippingPlanEntity.Id);

            if (result != null)
            {
                return 0;
            }

            _shippingAppDbContext.ShippingPlan.Add(shippingPlanEntity);
            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public List<Models.ShippingPlan> GetAllShippingPlan()
        {
            var shippingPlans = _shippingAppDbContext.ShippingPlan
                    .AsNoTracking()
                    .ToList();

            var results = _mapper.Map<List<Models.ShippingPlan>>(shippingPlans);

            return results.ToList();
        }

        public async Task<int> UpdateShippingPlan(Models.ShippingPlan shippingPlan)
        {
            var result = await _shippingAppDbContext.ShippingPlan.FindAsync(shippingPlan.Id);

            if (result == null)
            {
                return default;
            }

            _mapper.Map(shippingPlan, result);

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> DeleteShippingPlanByID(Guid Id)
        {
            var shippingPlan = await _shippingAppDbContext.ShippingPlan.FindAsync(Id);

            if (shippingPlan == null)
            {
                return default;
            }

            _shippingAppDbContext.ShippingPlan.Remove(shippingPlan);

            return await _shippingAppDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<Models.ShippingPlan> GetShippingPlanByID(Guid Id)
        {
            var result = await _shippingAppDbContext.ShippingPlan.FindAsync(Id);

            return _mapper.Map<Models.ShippingPlan>(result);
        }
    }
}

