using System.Linq.Expressions;
using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<bool> AddAsync(T entity)
        {
            if (entity == null)
            {
                return await Task.FromResult(false);
            }
            await dbSet.AddAsync(entity);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                return await Task.FromResult(false);
            }
            dbSet.Remove(entity);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteRangeAsync(T entities)
        {
            if (entities == null)
            {
                return await Task.FromResult(false);
            }
            dbSet.Remove(entities);
            return await Task.FromResult(true);
        }

    }
}
