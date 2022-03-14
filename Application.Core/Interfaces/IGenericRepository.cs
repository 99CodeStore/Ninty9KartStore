using NsdcTraingPartnerHub.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace NsdcTraingPartnerHub.Core.Interfaces
{

    public interface IGenericRepository<T> where T : class
    {
        Task<IPagedList<T>> GetPagedList(PagedRequest pagedRequest,
        Expression<Func<T, bool>> expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        List<string> includes = null);

        Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null
            );

        Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null
            );
        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);
        Task Delete(int Id);
        void DeleteRange(IEnumerable<T> Entities);
    }
}
