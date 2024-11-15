using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ThucHanhLanCuoi.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product product { get; set; }
        public int quantity { get; set; } = 1;

        // Tính toán EstimatedValue dựa trên giá sản phẩm và số lượng
        public decimal EstimatedValue => quantity * (product?.ProductPrice ?? 0);

        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }

        // Chỉnh sửa chính tả cho thuộc tính
        public IPagedList<Product> RelatedProducts { get; set; }
        public IPagedList<Product> TopProducts { get; set; }
    }
}