using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models;
using ThucHanhLanCuoi.Models.ViewModel;


namespace ThucHanhLanCuoi.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // GET: Admin/Users
        public ActionResult Index(string sortOrder)
        {
            //giá trị sắp sếp sẽ được lưu trữ trog viewbag sau đó truyền sang view 
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var users = db.Users.AsQueryable();

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.Username);
                    //order by Descending là 1 phương thức Linq dùng để sắp sếp giảm dần
                    break;
                default:
                    users = users.OrderBy(u => u.Username);
                    break;
            }

            return View(users.ToList());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            // nếu người dùng có ID = null trả về lõi HTTP 404

            var user = db.Users.Include("Customers").FirstOrDefault(u => u.Username == id);
            if (user == null)
                return HttpNotFound();

            var viewModel = new UserCustomerViewModel //tạo 1 view model hiển thị dữ liệu 
            {
                User = user,//truyền thông tin chi tiết người dùng user sang View
                Customers = user.Customers.ToList()
            };

            return View(viewModel);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,UserRole")] User user)
            //ánh xạ Bind tới các trường Username,Password,UserRole từ dữ liệu gửi lên đối tượng User
        {
            if (ModelState.IsValid) //nếu người dùng đã nhập dữ liệu các trường hợp lệ 
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Username == user.Username);//tạo 1 bản ghi tên đăng nhập
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                    return View(user);
                }

                user.UserRole = "N"; // Mặc định vai trò là Admin
                db.Users.Add(user); //thêm đói tượng người dùng là user vào Database User
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,UserRole")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                var relatedCustomers = db.Customers.Where(c => c.Username == user.Username).ToList();
                foreach (var customer in relatedCustomers)
                {
                    db.Customers.Remove(customer);
                }

                db.Users.Remove(user);
                db.SaveChanges();
            }
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
        public ActionResult Statistics()
        {
            // Tổng số lượng người dùng
            int totalUsers = db.Users.Count(); // Thay 'db.Users' bằng DbSet của bạn

            // Đếm số lượng Admin
            int adminCount = db.Users.Count(u => u.UserRole == "N");

            // Đếm số lượng Customer
            int customerCount = db.Users.Count(u => u.UserRole == "C");

            // Tính phần trăm
            double adminPercentage = totalUsers > 0 ? (adminCount * 100.0 / totalUsers) : 0;
            double customerPercentage = totalUsers > 0 ? (customerCount * 100.0 / totalUsers) : 0;

            // Gửi dữ liệu qua ViewBag
            ViewBag.AdminPercentage = Math.Round(adminPercentage, 2);
            ViewBag.CustomerPercentage = Math.Round(customerPercentage, 2);
            ViewBag.TotalUsers = totalUsers;
            ViewBag.AdminCount = adminCount;
            ViewBag.CustomerCount = customerCount;
            return View();
        }
    }
}