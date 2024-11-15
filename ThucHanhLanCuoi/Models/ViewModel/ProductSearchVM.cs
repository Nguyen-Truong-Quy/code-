using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThucHanhLanCuoi.Models.ViewModel
{
    public class ProductSearchVM
    {
        public string SearchTerm { get; set; } //Dùng để tìm kiếm sản phẩm


        //Các tiêu chí tìm kiếm theo giá tiền
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        //Sort
        public string SortOrder { get; set; }


        ////Dùng để lưu trữ dữ liệu product thành một danh sách tìm kiếm
        //public List<Product> Products { get; set; } 


        //Danh sách sản phẩm đã phân trang
        public PagedList.IPagedList<Product> Products { get; set; }

    }
}