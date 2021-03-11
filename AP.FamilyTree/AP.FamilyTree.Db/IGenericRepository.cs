using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AP.FamilyTree.Db
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Create(TEntity item);
        TEntity FindById(int id);
        IEnumerable<TEntity> Get();
        IQueryable<TEntity> GetQueryable();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> Take(int count);
        IEnumerable<TEntity> Take(int count, Func<TEntity, bool> predicate);
        void Remove(TEntity item);
        TEntity Update(TEntity item, string operation = "");
        TEntity Reload(int id);
    }
}
