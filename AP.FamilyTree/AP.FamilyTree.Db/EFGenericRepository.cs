using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AP.FamilyTree.Db
{
    public class EFRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;

        public EFRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }
        public IQueryable<TEntity> GetQueryable()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public IEnumerable<TEntity> Take(int count)
        {
            return _dbSet.Take(count).ToList();
        }

        public IEnumerable<TEntity> Take(int count, Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).Take(count).ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Create(TEntity item)
        {
            var itemNew = _dbSet.Add(item).Entity;
            _context.SaveChanges();
            return itemNew;
        }
        public TEntity Update(TEntity item, string operation = "")
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();

            _context.Entry(item).State = EntityState.Detached;
            _context.SaveChanges();
            return item;
        }

        public void Remove(TEntity item)
        {
            _dbSet.Attach(item);
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
        public TEntity Reload(int id)
        {
            var item = _dbSet.Find(id);
            _context.Entry(item).State = EntityState.Detached;
            var result = _context.Entry(item).GetDatabaseValues();
            if (result == null)
            {
                return null;
            }
            else
            {
                return (TEntity)result.ToObject();
            }
        }
    }
}
