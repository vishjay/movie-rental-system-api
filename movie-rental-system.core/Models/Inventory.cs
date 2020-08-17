using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class Inventory:BaseEntity
    {
        public Boolean Availability { get; set; }
        public Double Rent { get; set; }
        public Movie Movie { get; set; }

    }
}
