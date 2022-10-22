using BLOGN.Data.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLOGN.Data.Repositories.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbset; /* _dbSet bizim parametrelerimizi karşılayacak. */
        public Repository(ApplicationDbContext context )
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public async Task<bool> Add(T entity)
        {
           await _dbset.AddAsync(entity);
           return true;
        }

        public bool Delete(int id)
        {
            T entity = _dbset.Find(id);
            _dbset.Remove(entity);
            return true;
        }

        public async Task<T> Get(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task<ICollection<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = _dbset;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperties != null)
            {
                foreach(var iProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(iProperty);
                }
            }

            if(orderBy != null)
            {
                 return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var iProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(iProperty);
                }
            }          
            return await query.FirstOrDefaultAsync();
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
    }
}
