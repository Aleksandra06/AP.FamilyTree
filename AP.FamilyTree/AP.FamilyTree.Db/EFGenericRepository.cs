using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Interfaces;
using AP.FamilyTree.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace AP.FamilyTree.Db
{
    public class EFRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;
        private string _user;

        public EFRepository(DbContext context, string user = "")
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _user = user;
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().AsQueryable().ToList();
        }
        public IQueryable<TEntity> GetQueryable()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public IEnumerable<TEntity> Take(int count)
        {
            return _dbSet.AsNoTracking().Take(count).ToList();
        }

        public IEnumerable<TEntity> Take(int count, Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).Take(count).ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate);
        }

        public TEntity FindById(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
            {
                return null;
            }

            _context.Entry(entity).State = EntityState.Detached;
            _context.SaveChanges();

            return entity;
        }
        public TEntity FindByIdForReload(int id)
        {
            var item = _dbSet.Find(id);
            if (item != null)
            {
                _context.Entry(item).Reload();
            }

            return item;
        }
        public TEntity Create(TEntity item, string operation = "")
        {
            if (item is IChangeLog)
            {
                var changeLogJson = new List<ChangeLog>();
                changeLogJson.Add(new ChangeLog()
                {
                    Operation = String.IsNullOrEmpty(operation) ? "Create" : operation,
                    User = _user,
                    Date = DateTime.Now
                });
                ((IChangeLog)item).ChangeLogJson = JsonSerializer.Serialize(changeLogJson);
            }

            var itemNew = _dbSet.Add(item).Entity;
            _context.SaveChanges();

            _context.Entry(item).State = EntityState.Detached;
            _context.SaveChanges();

            return itemNew;
        }
        public TEntity Update(TEntity item, string operation = "")
        {
            if (item is IChangeLog)
            {
                var changeLogJson = string.IsNullOrEmpty(((IChangeLog)item).ChangeLogJson) ? new List<ChangeLog>() : JsonSerializer.Deserialize<List<ChangeLog>>(((IChangeLog)item).ChangeLogJson);
                changeLogJson.Add(new ChangeLog()
                {
                    Operation = String.IsNullOrEmpty(operation) ? "Update" : operation,
                    User = _user,
                    Date = DateTime.Now
                });
                ((IChangeLog)item).ChangeLogJson = JsonSerializer.Serialize(changeLogJson);
            }

            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();

            _context.Entry(item).State = EntityState.Detached;
            _context.SaveChanges();

            return item;
        }
        public TEntity Update(TEntity item, byte[] rowversion, string operation = "")
        {
            if (item is IsConcurrency)
            {
                _context.Entry(item).OriginalValues["RowVersion"] = rowversion;
            }

            if (item is IChangeLog)
            {
                var changeLogJson = string.IsNullOrEmpty(((IChangeLog)item).ChangeLogJson) ? new List<ChangeLog>() : JsonSerializer.Deserialize<List<ChangeLog>>(((IChangeLog)item).ChangeLogJson);
                changeLogJson.Add(new ChangeLog()
                {
                    Operation = String.IsNullOrEmpty(operation) ? "Update" : operation,
                    User = _user,
                    Date = DateTime.Now
                });
                ((IChangeLog)item).ChangeLogJson = JsonSerializer.Serialize(changeLogJson);
            }

            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();

            _context.Entry(item).State = EntityState.Detached;
            _context.SaveChanges();

            return item;
        }

        public void Remove(TEntity item)
        {
            try
            {
                _dbSet.Attach(item);
                _dbSet.Remove(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _context.Entry(item).State = EntityState.Detached;
                _context.SaveChanges();
                throw ex;
            }
        }
        public TEntity Reload(int id)
        {
            var item = _dbSet.Find(id);
            if (item == null)
            {
                return null;
            }
            _context.Entry(item).State = EntityState.Detached;
            var result = _context.Entry(item).GetDatabaseValues();

            return (TEntity)result?.ToObject();
        }
    }
}
