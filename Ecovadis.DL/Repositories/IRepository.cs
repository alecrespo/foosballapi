using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ecovadis.DAL.Models;

namespace Ecovadis.DL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<T> First(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Remove(T entity);
        void Remove(int id);
        void Update(T entity);
    }
}
