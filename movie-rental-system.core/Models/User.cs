using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class User:BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public Address MailingAddress { get; set; }
        public ActiveType Active { get; set; }
        public UserRole Role { get; set; }
        public LoyaltyLevel Loyalty { get; set; }
        public List<Rental> Rentals { get; set; }
        public List<RefreshToken> Refresh_Tokens { get; set; }

    }

    public enum ActiveType
    {
        [Display(Name = "Active")]
        Active = 0,
        [Display(Name = "Inactive")]
        Inactive = 1,
    }

    public enum LoyaltyLevel
    {
        Silver = 0,
        Gold = 1,
        Platinum = 2
    }

    public enum UserRole
    {
        [Display(Name = "Super Admin")]
        SuperAdmin = 0,
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Customer")]
        Customer = 2
    }



}

