using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThucHanhLanCuoi.Models.ViewModel
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // Calculates the total price for this item
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}