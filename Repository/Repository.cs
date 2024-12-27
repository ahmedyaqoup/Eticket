using Eticket.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Eticket.Repository.IRepository;

namespace Eticket.Repository
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly ApplicationDbcontext _dbContext;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbcontext dbContext)
        {
            this._dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Edit(T entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includeProps = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (includeProps != null)
            {
                foreach (var item in includeProps)
                {
                    query = query.Include(item);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public T? GetOne(Expression<Func<T, bool>>? filter, Expression<Func<T, object>>[]? includeProps = null, bool tracked = true)
        {
            return Get(filter,includeProps,tracked).FirstOrDefault();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();

        }
    }
}
