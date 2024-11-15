using System;
using System.Web.Mvc;
using ThucHanhLanCuoi.Models;
using ThucHanhLanCuoi.Models.ViewModel;

namespace ThucHanhLanCuoi.Controllers
{
    public class CartController : Controller
    {
        private QuanLyEntities db = new QuanLyEntities();

        // Đối tượng CartService khởi tạo trong constructor
        private CartService cartService;

        // Kiểm tra Session trong OnActionExecuting thay vì constructor
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Kiểm tra Session có sẵn không
            if (Session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }

            // Khởi tạo CartService
            if (cartService == null)
            {
                cartService = new CartService(Session);
            }
        }

        // Trả về CartService đã được khởi tạo
        private CartService GetCartService()
        {
            return cartService;
        }

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCartService().GetCart();
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ hàng
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                // Nếu không tìm thấy sản phẩm, trả về lỗi
                return HttpNotFound("Sản phẩm không tồn tại.");
            }

            var cartService = GetCartService();
            cartService.GetCart().AddItem(
                product.ProductID,
                product.ProductImage,
                product.ProductName,
                product.ProductPrice,
                quantity,
                product.Category.CategoryName
            );

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveFromCart(int id)
        {
            var cartService = GetCartService();
            cartService.GetCart().RemoveItem(id);
            return RedirectToAction("Index");
        }

        // Xóa tất cả sản phẩm trong giỏ hàng
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng của một sản phẩm trong giỏ
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cartService = GetCartService();
            cartService.GetCart().UpdateQuantity(id, quantity);
            return RedirectToAction("Index");
        }
    }
}
