using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class Address : BaseEntity
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<User> Residents { get; set; }
    }
}
