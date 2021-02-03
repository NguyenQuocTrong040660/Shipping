using ShippingApp.Domain.Interfaces;
using Models = ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities = ShippingApp.Domain.Entities;
using AutoMapper;
using ShippingApp.Domain.Models;
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
    public class PromotionRepository : IPromotionRepository
    {
        private readonly IShippingAppDbContext _productDbContext;
        private readonly IMapper _mapper;

        public PromotionRepository(IShippingAppDbContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> CreatePromotion(Promotion Promotion)
        {
            _productDbContext.Promotions.Add(new Entities.Promotion
            {
                Id = new Guid(),
                CustomerName = Promotion.CustomerName,
                PhoneNumber = Promotion.PhoneNumber,
                BirthDay = (DateTime)Promotion.BirthDay,
            });
            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> DeletePromotion(Guid Id)
        {
            var entity = await _productDbContext.Promotions.FindAsync(Id);

            if (entity == null)
            {
                return default;
            }

            _productDbContext.Promotions.Remove(entity);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public List<Promotion> GetAllPromotion()
        {
            List<Promotion> results = new List<Promotion>();
            var Promotions = _productDbContext.Promotions.ToList();

            foreach (var item in Promotions)
            {
                var model = _mapper.Map<Promotion>(item);
                results.Add(model);
            }
            return results;
        }

        public async Task<Promotion> GetPromotionByID(Guid id)
        {
            var result = await _productDbContext.Promotions.FindAsync(id);
            var model = _mapper.Map<Promotion>(result);
            return model;
        }

        public async Task<int> UpdatePromotion(Promotion entity)
        {
            var result = await _productDbContext.Promotions.FindAsync(entity.Id);

            if (result == null)
            {
                return default;
            }
            result.CustomerName = entity.CustomerName;
            result.BirthDay = (DateTime)entity.BirthDay;
            result.PhoneNumber = entity.PhoneNumber;
            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }
    }
}

