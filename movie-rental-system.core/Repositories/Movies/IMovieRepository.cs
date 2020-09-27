using movie_rental_system.core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Repositories
{
    public interface IMovieRepository:IRepository<Movie>
    {
        void SomeAdditionalMethod();
    }
}
