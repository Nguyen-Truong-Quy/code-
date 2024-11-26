using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models;
using PagedList;
using ThucHanhLanCuoi.Models.ViewModel;
using PagedList.Mvc;

namespace ThucHanhLanCuoi.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // GET: Admin/Products
        public ActionResult Index(string searchTerm, decimal? minPrice,
            decimal? maxPrice, string sortOrder, int? page)
        {
            var model = new ProductSearchVM();
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {   // Tìm kiếm sản phẩm dựa trên từ khóa
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                 p.ProductDescription.Contains(searchTerm) ||
                 p.Category.CategoryName.Contains(searchTerm));
            }
            // tìm kiếm sản phẩm dựa trên giá tối thiểu 
            if (minPrice.HasValue)
            {
                model.MinPrice = minPrice.Value;
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }
            // tìm kiếm sản phẩm dựa trên giá tối đa 
            if (maxPrice.HasValue)
            {
                model.MaxPrice = maxPrice.Value;
                products = products.Where(p => p.ProductPrice >= maxPrice.Value);
            }
            //Áp dụng sắp xếp dựa trên lựa chọn của người dùng
            switch (sortOrder)
            {
                case "name-asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "price-asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductDescription);
                    break;
                default://mạc đinh sắp xếp theo tên 
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }
            model.SortOrder = sortOrder;
            //Đoạn code liên quan đến phân trang
            //Lấy số trnag hiện tại(mặc định là trnag 1 nếu không có giá trị)
            int pageNumber = page ?? 1;
            int pageSize = 5; // số sản phẩm mỗi trang

            //Đóng câu lệnh này, sử dụng ToPageList để lấy dang sách đã phân trang
            //Model.Products=Products.Tolist()
            model.Products = products.ToPagedList(pageNumber, pageSize);
            return View(model); // Trả về model
        }
        //{
        //    var products = db.Products.Include(p => p.Category);
        //    return View(products.ToList());
        //}

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductImage,ProductDescription")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Loại bỏ dấu phẩy trong ProductPrice trước khi lưu
                product.ProductPrice = decimal.Parse(product.ProductPrice.ToString().Replace(",", string.Empty));

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,ProductPrice,ProductImage,ProductDescription")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Pass the product to the confirmation view
            return View(product);
        }
        // này là xóa hẳn không hoi 
        //public ActionResult Delete(int id)
        //{
        //    var product = db.Products.Find(id);
        //    if (product != null)
        //    {
        //        db.Products.Remove(product);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
      
          
       

    }
}
