using Microsoft.EntityFrameworkCore;
using movie_rental_system.core.Models;

namespace movie_rental_system.core.Data { 
    public class MovieRentalContext : DbContext
    {
        public MovieRentalContext(DbContextOptions<MovieRentalContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> InventoryItems { get; set; }

        public DbSet<InventoryRental> InventoryRentals { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieCategory> MovieCategories { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}