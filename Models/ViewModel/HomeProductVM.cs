using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThucHanhLanCuoi.Models;
using PagedList;
using PagedList.Mvc;

namespace ThucHanhLanCuoi.Models.ViewModel
{
    public class HomeProductVM
    {
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
        public IPagedList<Product> NewProducts { get; set; } // Sản phẩm mới
    }
}