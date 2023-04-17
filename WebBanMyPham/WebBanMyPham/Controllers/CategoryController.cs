using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;

namespace WebBanMyPham.Controllers
{
    public class CategoryController : Controller
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        // GET: Category
        public ActionResult Index()
        {
            var lstCategory = objWebBanMyPhamEntities.Category.ToList();
            return View(lstCategory);
        }
        public ActionResult ProductCategory(int Id)
        {
            this.LoadData();
            var lstCategory = objWebBanMyPhamEntities.Product.Where(n=>n.CategoryId== Id).ToList();
            return View(lstCategory);
        }
        void LoadData()
        {

            Common objCommon = new Common();
        
            ListtoDataTableConverter converter = new ListtoDataTableConverter();

            var lstBrand = objWebBanMyPhamEntities.Brand.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

        }
    }
}