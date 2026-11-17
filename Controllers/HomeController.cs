using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BackEndMyStore_25dh110949.Models;
using BackEndMyStore_25dh110949.Models.ViewModel;
using PagedList;

namespace BackEndMyStore_25dh110949.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                    p.ProductDescription.Contains(searchTerm) ||
                    p.Category.CategoryName.Contains(searchTerm));
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            model.FeaturedProducts = db.Products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();
            model.NewProducts = products.OrderBy(p => p.OrderDetails.Count()).Take(20).ToPagedList(pageNumber, pageSize);
                return View(model);
        }
    }
}