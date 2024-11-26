using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models;

namespace ThucHanhLanCuoi.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // GET: Admin/Categories
        // GET:Lấy dữ liệu từ bảng category trong DB để hiện thị lên

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        //Details:Lấy chi tiết 1 bản ghi có Category ID=id

        public ActionResult Details(int? id) //Với dấu hỏi chấm ở đây tức nghĩa là ta có thể truyền Id rỗng
        {
            if (id == null) //      Not found DB
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//Trả về mã lỗi 400 : Thiếu giá trị truyền vào
            }
            Category category = db.Categories.Find(id);//Nó sẽ tìm trong bản ghi nào khớp với dữ liệu id
            if (category == null)// Not found DB
            {
                return HttpNotFound();//    Trả về mã lỗi 404
            }
            return View(category);//Nếu tìm thấy trả về dữ liệu category tương ứng với id đó
        }

        // GET: Admin/Categories/Create
        //Load form CreatE
        //[Http Get] là phương thức mặc định,nên không cần khai báo từ khóa
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // POST: lưu dữ liệu từ form Create vào DB
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //Khi gửa dữ liệu sẽ được đẩy vào server 
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] Category category) //Thứ tự phải trùng với thứ tự trong bản ghi trong DB
        {
            if (ModelState.IsValid) //Nếu các trường dữ liệu ta nhập vào đúng 
            {
                db.Categories.Add(category); //Thêm 1 bản ghi
                db.SaveChanges(); //Lưu vào trong Model và Database
                return RedirectToAction("Index");
            }

            return View(category); // Sai thì load lại form view để chạy lại từ đầu
        }

        // GET: Admin/Categories/Edit/5
        // GET: Lấy dữ liệu 1 danh mục đã có sao cho CategoryID  = id;
        public ActionResult Edit(int? id)
        {
            //return Details(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            //Details(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
