using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models.ViewModel;
using ThucHanhLanCuoi.Models;
using PagedList.Mvc;
using PagedList;

namespace ThucHanhLanCuoi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult TrangChu()
        {
            return View();
        }

        private QuanLyEntities db = new QuanLyEntities();

        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                 p.ProductDescription.Contains(searchTerm) ||
                 p.Category.CategoryName.Contains(searchTerm));
            }
            int pageNumber = page ?? 1;
            int pageSize = 6;
            model.FeaturedProducts = products
                .OrderByDescending(p => p.OrderDetails
                .Count()).Take(5)
                .ToList();

            model.NewProducts = products.OrderBy(p => p.OrderDetails
            .Count()).Take(20)
            .ToPagedList(pageNumber, pageSize);
            return View(model);
        }


        // -------------------------------------------------

        // get : home / ProductDetails/5
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

            // Lấy tất cả các sản phẩm cùng danh mục (trừ sản phẩm hiện tại)
            var products = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();

            ProductDetailVM model = new ProductDetailVM();

            // Cung cấp giá trị cho số trang hiện tại (trang 1 mặc định nếu không có giá trị)
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize; // 8 sản phẩm mỗi trang

            // Gán sản phẩm cho model
            model.product = pro;

            // Lấy danh sách sản phẩm liên quan (cùng danh mục)
            model.RelatedProducts = products.OrderBy(p => p.ProductID).ToPagedList(pageNumber, pageSize);

            // Lấy các sản phẩm top (ví dụ theo số lượng đã bán)
            model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count()).ToPagedList(pageNumber, pageSize);

            // Gán số lượng nếu có
            if (quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }

            // Trả về View với model đã được chuẩn bị
            return View(model);
        }


        //model.RelatedProducts = products.OrderBy(p => p.ProductID).Take(8).ToPagedList(pageNumber, pageSize);
        //model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(8).ToPagedList(pageNumber, pageSize);

        public ActionResult TrangNike()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangBlog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LienHe()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangAdidas()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangAsics()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangGiayTheThao()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangLacoste()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangPuma()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TrangSale()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ChinhSachDoiTra()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}