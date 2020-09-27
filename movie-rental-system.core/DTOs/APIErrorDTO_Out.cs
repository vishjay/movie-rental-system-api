using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.DTOs
{
    public class APIErrorDTO_Out
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public string Details { get; set; }

    }
}
