using movie_rental_system.core.Data;
using movie_rental_system.core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {   
        public MovieRepository(MovieRentalContext context): base(context)
        {

        }
        public void SomeAdditionalMethod()
        {
            throw new NotImplementedException();
        }
    }

}
