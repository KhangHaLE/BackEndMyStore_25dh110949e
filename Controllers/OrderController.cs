using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BackEndMyStore_25dh110949.Models;
using BackEndMyStore_25dh110949.Models.ViewModel;

namespace BackEndMyStore_25dh110949.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Order/MyOrder
        public ActionResult MyOrder(string searchTerm)
        {
            // 1. Lấy tên đăng nhập từ Session hoặc User.Identity (Tránh lỗi cast IPrincipal)
            string username = null;

            if (Session["Username"] != null)
            {
                username = Session["Username"].ToString();
            }
            else if (Session["User"] != null)
            {
                username = Session["User"].ToString();
            }
            else if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                username = User.Identity.Name;
            }

            // Nếu chưa đăng nhập -> chuyển hướng về trang Login
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Truy vấn danh sách đơn hàng của tài khoản hiện tại, sắp xếp mới nhất lên đầu
            // LƯU Ý: Đổi "o.Username" thành tên cột thực tế trong bảng Order của bạn (VD: o.User.Username hoặc o.Email)
            var query = db.Orders
                          .Where(o => o.Username == username)
                          .OrderByDescending(o => o.OrderDate)
                          .AsQueryable();

            // 3. Lọc theo từ khóa tìm kiếm (Mã đơn hàng hoặc Tên sản phẩm)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string keyword = searchTerm.Trim().ToLower();
                query = query.Where(o => o.OrderID.ToString().Contains(keyword) ||
                                         o.OrderDetails.Any(od => od.Product.ProductName.ToLower().Contains(keyword)));
            }

            var ordersList = query.ToList();

            // 4. Đổ dữ liệu vào ViewModel theo từng trạng thái (Giới hạn tối đa 50 đơn gần nhất)
            var viewModel = new MyOrder
            {
                SearchTerm = searchTerm,
                AllOrders = ordersList.,
                PendingPaymentOrders = ordersList.Where(o => o.PaymentStatus == "Chờ thanh toán").Take(50).ToList(),
                ProcessingOrders = ordersList.Where(o => o.OrderD == "Đang xử lý").Take(50).ToList(),
                ShippingOrders = ordersList.Where(o => o.OrderStatus == "Đang vận chuyển").Take(50).ToList(),
                DeliveredOrders = ordersList.Where(o => o.OrderStatus == "Đã giao").Take(50).ToList(),
                CancelledOrders = ordersList.Where(o => o.OrderStatus == "Đã hủy").Take(50).ToList()
            };

            return View(viewModel);
        }

        // Action xử lý nút MUA LẠI: Thêm sản phẩm vào giỏ hàng với số lượng = 1
        public ActionResult BuyAgain(int productId)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Gọi lại logic thêm vào giỏ hàng hiện có của bạn với quantity = 1
            // Ví dụ: CartService.AddToCart(productId, 1);

            return RedirectToAction("Index", "Cart");
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