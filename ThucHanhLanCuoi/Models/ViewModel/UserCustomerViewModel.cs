using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThucHanhLanCuoi.Models.ViewModel
{
    public class UserCustomerViewModel
    {
        public User User { get; set; }  
        public List<Customer> Customers { get; set; }
    }
}