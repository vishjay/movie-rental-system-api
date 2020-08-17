using movie_rental_system.core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.DTOs
{
    public class AuthUserDTO_Out
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Address MailingAddress { get; set; }
        public ActiveType Active { get; set; }
        public UserRole Role { get; set; }
        public LoyaltyLevel Loyalty { get; set; }
    }
}
