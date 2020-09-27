using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace movie_rental_system.core.Repositories
{
    public interface IRepository<T> where T : class
    {   
        //fetching data
        T Get(Guid Id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        //add data
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        //remove data
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
