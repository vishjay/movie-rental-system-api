using movie_rental_system.core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace movie_rental_system.core.DTOs
{
    public class MovieDTO_In
    {   
        [Required]
        public string Name { get; set; }
        public Language Language { get; set; }
        public string ImageURL { get; set; }
        public List<Guid> CategoryIds { get; set; }

    }
}
