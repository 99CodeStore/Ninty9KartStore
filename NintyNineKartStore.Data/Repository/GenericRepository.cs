using Microsoft.EntityFrameworkCore;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace NintyNineKartStore.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Ninty9KartStoreDbContext dbContext;
        private readonly DbSet<T> db;

        public GenericRepository(Ninty9KartStoreDbContext context)
        {
            this.dbContext = context;
            db = context.Set<T>();

        }
        public async Task Delete(int Id)
        {
            var entity = await db.FindAsync(Id);
            db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> Entities)
        {
            db.RemoveRange(Entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = db;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IPagedList<T>> GetPagedList( PagedRequest pagedRequest,
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null)
        {
            IQueryable<T> query = db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToPagedListAsync(pagedRequest.PageNumber,pagedRequest.PageSize);
        }

        public async Task Insert(T entity)
        {
            await db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await db.AddRangeAsync(entities);
        }

        //public void Update(T entity)
        //{
        //    db.Attach(entity);
        //    dbContext.Entry(entity).State = EntityState.Modified;
        //}
    }
}
