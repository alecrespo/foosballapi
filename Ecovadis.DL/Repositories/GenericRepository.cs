using Ecovadis.DAL.Contexts;
using Ecovadis.DAL.Models;
using Ecovadis.DL.Infrastructures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecovadis.DL.Repositories
{
    public class GenericRepository<T> : IDisposable, IRepository<T> where T : BaseEntity
    {
        private readonly EcovadisContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private readonly IErrorHandler _errorHandler;

        public GenericRepository(EcovadisContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync<T>(predicate);
        }

        public async Task<T> First(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstAsync<T>(predicate);
        }
        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            try
            {
                _dbSet.Add(entity);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            try
            {
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void Remove(int id)
        {
            var entity = Get(id).Result;
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            Remove(entity);
        }
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.EntityNull), "", "Input data is null"));
            try
            {
                var oldEntity = _dbContext.FindAsync<T>(entity.Id).Result;
                var rowversion = oldEntity.RowVersion;
                _dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.RowVersion = rowversion;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> Get(int id)
        {
            return await _dbSet.SingleOrDefaultAsync(s => s.Id == id);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

    }
}
