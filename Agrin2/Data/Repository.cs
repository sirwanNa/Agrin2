using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agrin2.Entities;
using Microsoft.EntityFrameworkCore;


namespace Agrin2.Data
{
    public abstract class Repository<TEntity> where TEntity : BaseEntity
    {
        private DbContext _context;
        public Repository(IUnitOfWork unitOfWork)
        {
            _context = unitOfWork.Context;
        }
        public virtual void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> objects = _context.Set<TEntity>().Where<TEntity>(where).AsEnumerable();
            foreach (TEntity obj in objects)
                _context.Set<TEntity>().Remove(obj);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public virtual void Edit(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(predicate);
            return query;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            return query;
        }
        public virtual TEntity GetById(Guid id)
        {
            return (from i in GetAll().Where(i => i.Id == id) select i).SingleOrDefault();
        }    
    }
}
