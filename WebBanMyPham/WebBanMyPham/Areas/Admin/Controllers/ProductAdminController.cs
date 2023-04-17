using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;
using WebBanMyPham.DAO;
using System.Web.Services.Description;

namespace WebBanMyPham.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        // GET: Admin/ProductAdmin
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
       productDao productdao=new productDao();
        // GET: Admin/Product
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
           this.LoadData();
            var lstproduct = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstproduct = objWebBanMyPhamEntities.Product.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstproduct = objWebBanMyPhamEntities.Product.Where(n=>n.ShowOnHomePage==true).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstproduct = lstproduct.OrderByDescending(n => n.Id).ToList();
            return View(lstproduct.ToPagedList(pageNumber, pageSize));
            // var lstPD=objWebBanMyPhamEntities.Product.Where(n=>n.Name.Contains(SearchString)).ToList(); 
            //return View(lstPD);
        }

        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                        string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        objProduct.Avatar = fileName;
                        objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product/"), fileName));
                    }
                    objProduct.CreatedOnUtc = DateTime.Now;
                    objWebBanMyPhamEntities.Product.Add(objProduct);
                    objWebBanMyPhamEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var lstProduct = objWebBanMyPhamEntities.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(lstProduct);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = objWebBanMyPhamEntities.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objWebBanMyPhamEntities.Product.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Trash");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var objProduct = objWebBanMyPhamEntities.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(int id, Product objProduct)
        {
            this.LoadData();
            if (objProduct.ImageUpload != null)
            {
                String fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                String extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/product/"), fileName));

            }
            objWebBanMyPhamEntities.Entry(objProduct).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        void LoadData()
        {

            Common objCommon = new Common();
            var lstCat = objWebBanMyPhamEntities.Category.ToList();
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            var lstBrand = objWebBanMyPhamEntities.Brand.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);
            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }

        public ActionResult Trash()
        {
            
            var objProduct = objWebBanMyPhamEntities.Product.Where(n => n.ShowOnHomePage == false).ToList();
            return View("Trash", objProduct);
        }
        public ActionResult Destroy(int? id,Product objPro)
        {
            //string ShowOnHomePage = "All";
            var objProduct = objWebBanMyPhamEntities.Product.Where(n => n.Id == objPro.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
             objProduct.ShowOnHomePage = false;
           
            objProduct.CreatedOnUtc= DateTime.Now;
            objProduct.UpdatedOnUtc= DateTime.Now;
           
           // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objProduct).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Index");
        }
        public ActionResult Restore(int? id, Product objPro)
        {
            //string ShowOnHomePage = "All";
            var objProduct = objWebBanMyPhamEntities.Product.Where(n => n.Id == objPro.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
            objProduct.ShowOnHomePage = true;

            objProduct.CreatedOnUtc = DateTime.Now;
            objProduct.UpdatedOnUtc = DateTime.Now;

            // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objProduct).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Trash","ProductAdmin");
        }


    }
}