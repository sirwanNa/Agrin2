using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Agrin2.Data
{
    public interface IUnitOfWork 
    {
        DbContext Context { get; set; }
       // IEnumerable<T> ReadRawSql<T>(string query);
        void Save();
    }
}
