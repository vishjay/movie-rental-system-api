using Microsoft.EntityFrameworkCore;
using movie_rental_system.core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace movie_rental_system.core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MovieRentalContext context;
        private readonly DbSet<T> entities;

        public Repository(MovieRentalContext context)
        {
            this.context = context;
            this.entities = this.context.Set<T>();
        }

        public void Add(T entity)
        {
            this.entities.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            this.entities.AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.entities.Where(predicate);
        }

        public T Get(Guid Id)
        {
            return this.entities.Find(Id);
        }

        public IEnumerable<T> GetAll()
        {
            return this.entities.ToList();
        }

        public void Remove(T entity)
        {
            this.entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            this.entities.RemoveRange(entities);
        }
    }
}
