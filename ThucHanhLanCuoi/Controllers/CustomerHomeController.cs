using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models.ViewModel;
using ThucHanhLanCuoi.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;

namespace ThucHanhLanCuoi.Controllers
{
    public class CustomerHomeController : Controller
    {
        // Sử dụng một dbContext duy nhất để truy cập vào dữ liệu
        private readonly QuanLyEntities db;

        // Khởi tạo dbContext trong constructor
        public CustomerHomeController()
        {
            db = new QuanLyEntities();  // Đảm bảo db là dbContext của bạn
        }

        // Action Index hiển thị sản phẩm và phân trang
        public ActionResult TrangChu_Customer(string SearchTerm, int? page)
        {
            var model = new HomeProductVM
            {
                FeaturedProducts = new List<Product>(), // Khởi tạo FeaturedProducts để tránh NullReferenceException
                NewProducts = null // Đặt NewProducts ban đầu là null, sẽ gán lại bên dưới sau khi chuyển đổi
            };

            // Khởi tạo truy vấn sản phẩm
            var productsQuery = db.Products.Include(p => p.Category).Include(p => p.OrderDetails).AsQueryable();

            // Tìm kiếm sản phẩm theo từ khóa
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                model.SearchTerm = SearchTerm;
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(SearchTerm)
                                                         || p.ProductDescription.Contains(SearchTerm)
                                                         || p.Category.CategoryName.Contains(SearchTerm));
            }

            // Lấy top 10 sản phẩm bán chạy nhất cho FeaturedProducts
            model.FeaturedProducts = productsQuery
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(10)
                .ToList();

            // Đoạn code lấy danh sách sản phẩm mới với phân trang
            int pageNumber = page ?? 1; 
            int pageSize = 6;  
            model.NewProducts = productsQuery
          .AsEnumerable()
          .OrderByDescending(p => p.DateAdded)
           .ToPagedList(pageNumber, pageSize);


            return View(model);  // Trả về view với model đã được gán đầy đủ dữ liệu
        }

        // Action TrangChu_Customer - không có nội dung xử lý trong ví dụ này


        // Đảm bảo đóng kết nối dbContext trong controller
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();  // Đóng kết nối db khi controller không sử dụng nữa
            }
            base.Dispose(disposing);
        }

        // GET: Home/ProductDetails/5
        public ActionResult ProductDetails(int? id, int? quantity, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = db.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }

            // Lấy tất cả các sản phẩm cùng danh mục
            var products = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();

            ProductDetailVM model = new ProductDetailVM
            {
                product = pro,
                PageSize = 8, // Số lượng mặc định là 8 sản phẩm mỗi trang
                quantity = quantity ?? 1 // Đặt số lượng nếu không có giá trị
            };

            // Phân trang cho sản phẩm liên quan        
            int pageNumber = page ?? 1;
            model.RelatedProducts = products.OrderBy(p => p.ProductID).ToPagedList(pageNumber, model.PageSize);

            return View(model);
        }
        public ActionResult Register()
        {
            return View(); // Phải trả về View() để tìm đúng Register.cshtml
        }
    }
}