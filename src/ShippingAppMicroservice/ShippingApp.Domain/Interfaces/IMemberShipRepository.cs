using ShippingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShippingApp.Domain.Interfaces
{
    public interface IMemberShipRepository
    {
        List<MemberShip> GetAllMemberShip();
        Task<MemberShip> GetMemberShipByID(Guid id);
        Task<int> CreateMemberShip(MemberShip memberShip);
        Task<int> DeleteMemberShip(Guid Id);
        Task<int> UpdateMemberShip(MemberShip entity);
    }
}
