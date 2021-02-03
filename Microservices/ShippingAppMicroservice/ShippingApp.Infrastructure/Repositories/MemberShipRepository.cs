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
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly IShippingAppDbContext _productDbContext;
        private readonly IMapper _mapper;

        public MemberShipRepository(IShippingAppDbContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> CreateMemberShip(MemberShip memberShip)
        {
            _productDbContext.MemberShips.Add(new Entities.MemberShip
            {
                Id = new Guid(),
                CustomerName = memberShip.CustomerName,
                PhoneNumber = memberShip.PhoneNumber,
                BirthDay = (DateTime)memberShip.BirthDay,
            });
            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> DeleteMemberShip(Guid Id)
        {
            var entity = await _productDbContext.MemberShips.FindAsync(Id);

            if (entity == null)
            {
                return default;
            }

            _productDbContext.MemberShips.Remove(entity);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public List<MemberShip> GetAllMemberShip()
        {
            List<MemberShip> results = new List<MemberShip>();
            var memberShips = _productDbContext.MemberShips.ToList();

            foreach (var item in memberShips)
            {
                var model = _mapper.Map<MemberShip>(item);
                results.Add(model);
            }
            return results;
        }

        public async Task<MemberShip> GetMemberShipByID(Guid id)
        {
            var result = await _productDbContext.MemberShips.FindAsync(id);
            var model = _mapper.Map<MemberShip>(result);
            return model;
        }

        public async Task<int> UpdateMemberShip(MemberShip entity)
        {
            var result = await _productDbContext.MemberShips.FindAsync(entity.Id);

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

