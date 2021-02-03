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
    public class ReservationRepository : IReservationRepository
    {
        private readonly IShippingAppDbContext _productDbContext;
        private readonly IMapper _mapper;

        public ReservationRepository(IShippingAppDbContext productDbContext, IMapper mapper)
        {
            _productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> CreateReservation(Reservation reservation)
        {

            _productDbContext.Reservations.Add(new Entities.Reservation
            {
                Id = new Guid(),
                CustomerName = reservation.CustomerName,
                PhoneNumber = reservation.PhoneNumber,
                Email = reservation.Email,
                BirthDay = (DateTime)reservation.BirthDay,
                DateSet = (DateTime)reservation.DateSet,
                //TimeSet = (TimeSpan)reservation.TimeSet,
                CountPerson = reservation.CountPerson,
                ServiceName = reservation.ServiceName
            });

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public async Task<int> DeleteReservation(Guid Id)
        {
            var entity = await _productDbContext.Reservations.FindAsync(Id);

            if (entity == null)
            {
                return default;
            }

            _productDbContext.Reservations.Remove(entity);

            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }

        public List<Reservation> GetAllReservation()
        {
            List<Reservation> results = new List<Reservation>();
            var reservations = _productDbContext.Reservations.ToList();

            foreach (var item in reservations)
            {
                var model = _mapper.Map<Reservation>(item);
                results.Add(model);
            }

            return results;
        }

        public async Task<Reservation> GetReservationByID(Guid id)
        {
            var result = await _productDbContext.Reservations.FindAsync(id);
            var model = _mapper.Map<Reservation>(result);
            return model;
        }

        public async Task<int> UpdateReservation(Reservation entity)
        {
            var result = await _productDbContext.Reservations.FindAsync(entity.Id);

            if (result == null)
            {
                return default;
            }
            result.CustomerName = entity.CustomerName;
            result.BirthDay = (DateTime)entity.BirthDay;
            result.DateSet = (DateTime)entity.DateSet;
            result.PhoneNumber = entity.PhoneNumber;
            result.Email = entity.Email;
            result.CountPerson = entity.CountPerson;
            result.ServiceName = entity.ServiceName;
            return await _productDbContext.SaveChangesAsync(new CancellationToken());
        }
    }
}

