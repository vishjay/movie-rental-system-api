using movie_rental_system.core.Enums;
using movie_rental_system.core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.DTOs
{
    public class MovieDTO_Out
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Language Language { get; set; }
        public string ImageURL { get; set; }
        public List<CategoryDTO_Out> Categories { get; set; }
    }
}
