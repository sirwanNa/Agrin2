using Agrin2.Entities;
using System;
using System.Linq;

namespace Agrin2.Data
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity entity);
        void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> where);
        void Delete(TEntity entity);
        void Edit(TEntity entity);
        IQueryable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        TEntity GetById(Guid id);       

    }
}
