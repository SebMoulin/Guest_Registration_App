using System;
using System.Collections.Generic;

namespace Company.Welcome.Ral.GuestVisitor
{
    public interface ITekGuestVisitorRepository<TEntity>
    {
        TEntity GetbyId(Guid id);
        IEnumerable<TEntity> GetAll();
        void Delete(Guid id);
        void InsertOrUpdate(TEntity entity);
    }
}