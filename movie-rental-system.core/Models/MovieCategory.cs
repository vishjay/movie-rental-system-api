using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class MovieCategory: BaseEntity
    {
        public Movie Movie { get; set; }
        public Category Category { get; set; }
    }
}
