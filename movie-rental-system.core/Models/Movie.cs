using movie_rental_system.core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class Movie:BaseEntity
    {
        public string Name { get; set; }
        public Language MovieLanguage { get; set; }
        public string ImageURL { get; set; }
        public List<Inventory> InventoryItems { get; set; }
        public List<MovieCategory> Categories { get; set; }
    }

}
