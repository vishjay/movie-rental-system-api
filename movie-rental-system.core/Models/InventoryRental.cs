using System;
using System.Collections.Generic;
using System.Text;

namespace movie_rental_system.core.Models
{
    public class InventoryRental:BaseEntity
    {
        public Inventory InventoryItem { get; set; }
        public Rental RentalInvoice { get; set; }
    }
}
