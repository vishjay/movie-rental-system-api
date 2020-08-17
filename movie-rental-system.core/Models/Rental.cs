using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class Rental:BaseEntity
    {
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Double Total { get; set; }
        public User Customer { get; set; }
    }
}
