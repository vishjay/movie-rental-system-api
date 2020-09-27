using movie_rental_system.core.Data;
using movie_rental_system.core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.UnitofWork
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly MovieRentalContext context;
        public UnitOfWork(MovieRentalContext context)
        {
            this.context = context;
            this.Movies = new MovieRepository(this.context);
        }
        public IMovieRepository Movies { get; }
        public void Complete()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
