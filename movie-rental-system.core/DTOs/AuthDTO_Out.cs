using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace movie_rental_system.core.DTOs
{
    public class AuthDTO_Out
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }

    }
}
