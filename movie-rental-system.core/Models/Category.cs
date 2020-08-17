using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public List<MovieCategory> MoviesInCategory { get; set; }
    }
}
