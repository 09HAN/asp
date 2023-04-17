using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;
using WebBanMyPham.Models;

namespace WebBanMyPham.Controllers
{
    public class ListingGridController : Controller
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        // GET: ListingGrid
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListProduct = objWebBanMyPhamEntities.Product.ToList();
            return View(objHomeModel);
        }
    }
}