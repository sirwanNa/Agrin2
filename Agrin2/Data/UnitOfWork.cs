using Microsoft.EntityFrameworkCore;

namespace Agrin2.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DbContext context)
        {
            Context = context;
        }
        public DbContext Context { get; set; }
        //public IEnumerable<T> ReadRawSql<T>(string query)
        //{
        //    return Context.Database.SqlQuery<T>(query);
        //}
        public void Save()
        {
            Context.SaveChanges();
        }

    }
}
