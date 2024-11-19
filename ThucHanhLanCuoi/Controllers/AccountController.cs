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
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Lưu trạng thái đăng nhập vào Session
                  
                    Session["Username"] = user.Username;

                    // Kiểm tra vai trò và chuyển hướng tương ứng
                    if (user.UserRole == "N") // Admin
                    {
                        return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                    }
                    else
                    {
                        Session["login"] = true;
                        return RedirectToAction("TrangChu_Customer", "CustomerHome");
                    }
                }
                else
                {
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
            // Kiểm tra nếu người dùng đã đăng nhập
            if (Session["Username"] != null)
            {
                string username = Session["Username"].ToString();
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    return View(user);  // Trả về thông tin người dùng
                }
            }
            return RedirectToAction("Login", "Account");
        }
        // GET: Account/ChangePassword
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng từ Session
                var username = User.Identity.Name; // Tên người dùng đang đăng nhập
                var user = db.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    // Kiểm tra mật khẩu cũ
                    if (user.Password == model.OldPassword)
                    {
                        // Cập nhật mật khẩu mới
                        user.Password = model.NewPassword;
                        db.SaveChanges();

                        TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công.";
                        return RedirectToAction("TrangTaiKhoan", "Account"); // Chuyển hướng đến trang tài khoản
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu cũ không đúng.");
                    }
                }
            }

            return View(model);
        }
    }
}