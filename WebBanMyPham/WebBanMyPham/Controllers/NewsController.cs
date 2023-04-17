using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;

namespace WebBanMyPham.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        public ActionResult Index()
        {
            var lstPost = objWebBanMyPhamEntities.Posts.ToList();
            return View(lstPost);
        }
    }
}