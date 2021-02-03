using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShippingApp.Domain.Interfaces
{
    public interface IPromotionRepository
    {
        List<Promotion> GetAllPromotion();
        Task<Promotion> GetPromotionByID(Guid id);
        Task<int> CreatePromotion(Promotion Promotion);
        Task<int> DeletePromotion(Guid Id);
        Task<int> UpdatePromotion(Promotion entity);
    }
}
