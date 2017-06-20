using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.Welcome.Entities.GuestVisitor;

namespace Company.Welcome.Business.GuestVisitor
{
    public interface ITekGuestVisitorBusinessService
    {
        Task<IEnumerable<Visitor>> GetAllGuest(DateTime date);
        Task<Visitor> GetGuestDetails(Guid guestId);
        Task<bool> SaveGuest(Visitor visitor);
        Task<bool> GuestIsLeaving(Guid guestId, DateTime departure);
    }
}