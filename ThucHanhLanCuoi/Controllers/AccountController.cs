using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThucHanhLanCuoi.Models.ViewModel;
using ThucHanhLanCuoi.Models;
using PagedList.Mvc;

namespace ThucHanhLanCuoi.Controllers
{
    public class AccountController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // kiem tra ten dang nhap
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }

                // neu chua ton tai thi tao ban ghi thong tin tk trong bang user
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    UserRole = "C"

                };
                db.Users.Add(user);
                // va tao ban ghi thong tin khach hang trong bang customer
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };
                db.Customers.Add(customer);
                // luu thong tin tai khoan vao csdl
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                }
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }
       

        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra thông tin đăng nhập (username, password)
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Kiểm tra vai trò của người dùng
                    if (user.UserRole == "N") // Nếu là admin
                    {
                        // Lưu thông tin người dùng vào Session hoặc Cookie (tùy thuộc vào yêu cầu)
                        Session["UserRole"] = user.UserRole;
                        Session["Username"] = user.Username;

                        // Chuyển hướng đến trang AdminHome/Index
                        return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                    }
                    else
                    {
                        // Nếu không phải là admin, chuyển hướng đến trang người dùng bình thường
                        return RedirectToAction("TrangChu_Customer", "CustomerHome");
                    }
                }
                else
                {
                    // Nếu thông tin đăng nhập không hợp lệ
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        //GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult InfoAccount()
        {
            return View();
        }

    }
}