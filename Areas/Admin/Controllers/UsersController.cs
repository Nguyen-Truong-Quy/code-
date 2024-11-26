using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models;

namespace ThucHanhLanCuoi.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create([Bind(Include = "Username,Password,UserRole")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = db.Users.FirstOrDefault(u => u.Username == user.Username);
                    if (existingUser != null)
                    {
                        // Thêm lỗi vào ModelState nếu Username đã tồn tại
                        ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                        return View(user);
                    }
                    // Gán vai trò mặc định là "N" (admin) khi tạo tài khoản mới.
                    user.UserRole = "N"; // Vai trò "N" cho admin, có thể điều chỉnh nếu cần

                    // Thêm người dùng mới vào cơ sở dữ liệu
                    db.Users.Add(user);
                    db.SaveChanges();

                    // Chuyển hướng về trang danh sách người dùng
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException dbEx)
                {
                    // Kiểm tra lỗi validation từ Entity Framework
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationErrors.Entry.Entity.GetType().Name,
                                validationError.ErrorMessage);
                        }
                    }
                }
            }

            // Trả về View nếu có lỗi
            return View(user);
        }


        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
