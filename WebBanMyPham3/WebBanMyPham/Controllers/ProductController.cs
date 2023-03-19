using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;

namespace WebBanMyPham.Controllers
{
    public class ProductController : Controller
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        // GET: Product
        public ActionResult Detail(int id)
        {
            var product=objWebBanMyPhamEntities.Product.Where(p=> p.Id == id).FirstOrDefault();
            return View(product);
        }
    }
}