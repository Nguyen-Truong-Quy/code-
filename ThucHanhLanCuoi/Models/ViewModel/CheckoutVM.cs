using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThucHanhLanCuoi.Models.ViewModel
{
    public class CheckoutVM  // lưu thông tin from checkout
    {
        public List<CartItem> CartItems { get; set; }
        public int CustomerID { get; set; }
        [Display(Name = "Ngày đặt hàng")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Tổng giá trị")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thứ giao hàng")]
        public string ShippingMethod { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        public string Username { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}