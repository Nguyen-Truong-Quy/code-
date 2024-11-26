using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models;
using ThucHanhLanCuoi.Models.ViewModel;

namespace ThucHanhLanCuoi.Controllers
{
    public class AccountController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // GET: Account/Register
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
                // Kiểm tra tên đăng nhập
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại!");
                    return View(model);
                }

                // Nếu chưa tồn tại, tạo bản ghi thông tin tài khoản trong bảng User
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,  // Mật khẩu không mã hóa
                    UserRole = "C"
                };
                db.Users.Add(user);

                // Tạo bản ghi thông tin khách hàng trong bảng Customer
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };
                db.Customers.Add(customer);

                // Lưu thông tin vào cơ sở dữ liệu
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

        // GET: Account/Login
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
                // Tìm người dùng với tên đăng nhập và mật khẩu (không mã hóa mật khẩu)
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

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // GET: Account/InfoAccount
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
                // Lấy user hiện tại dựa trên thông tin đăng nhập
                var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
                if (user == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                    return View(model);
                }

                // Kiểm tra mật khẩu cũ
                if (user.Password != model.OldPassword)
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng.");
                    return View(model);
                }

                // Gán mật khẩu mới
                user.Password = model.NewPassword;

                // Lưu thay đổi
                try
                {
                    db.SaveChanges();  // Lưu vào cơ sở dữ liệu

                    // Kiểm tra nếu đã lưu thành công
                    if (db.Entry(user).State == EntityState.Modified)
                    {
                        // Đổi mật khẩu thành công
                        TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";

                        // Xóa session cũ và yêu cầu người dùng đăng nhập lại
                        Session.Clear();

                        // Chuyển hướng đến trang đăng nhập
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không thể cập nhật mật khẩu!";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi khi lưu mật khẩu: " + ex.Message);
                }
            }

            return View(model);
        }
    }
}
