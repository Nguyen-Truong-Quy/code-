using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThucHanhLanCuoi.Models.ViewModel
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items => items;

        // Add an item to the cart
        public void AddItem(int productId, string productImage, string productName, decimal unitPrice, int quantity, string category)
        {
            var existingItem = items.FirstOrDefault(i => i.ProductID == productId);

            if (existingItem == null)
            {
                items.Add(new CartItem
                {
                    ProductID = productId,
                    ProductImage = productImage,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    Quantity = quantity
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        // Remove an item from the cart by product ID
        public void RemoveItem(int productId)
        {
            items.RemoveAll(i => i.ProductID == productId);
        }

        // Calculate the total value of the cart
        public decimal TotalValue()
        {
            return items.Sum(i => i.TotalPrice);
        }

        // Clear all items from the cart
        public void Clear()
        {
            items.Clear();
        }

        // Update the quantity of a specific item
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }
    }
}