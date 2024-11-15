using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThucHanhLanCuoi.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: Admin/AdminHome
        // GET: Admin/AdminHome/Index
        public ActionResult Index()
        {
            // Chỉ cho phép admin truy cập
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "N")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" }); // Chuyển hướng về trang đăng nhập
            }
        }
    }
}