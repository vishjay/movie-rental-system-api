using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace movie_rental_system.core.DTOs
{
    public class CategoryDTO_In
    {   
        [Required]
        public string CategoryName { get; set; }
    }
}
