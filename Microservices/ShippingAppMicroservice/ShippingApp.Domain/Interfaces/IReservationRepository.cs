using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShippingApp.Domain.Interfaces
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservation();
        Task<Reservation> GetReservationByID(Guid id);
        Task<int> CreateReservation(Reservation reservation);
        Task<int> DeleteReservation(Guid Id);
        Task<int> UpdateReservation(Reservation entity);
    }
}
