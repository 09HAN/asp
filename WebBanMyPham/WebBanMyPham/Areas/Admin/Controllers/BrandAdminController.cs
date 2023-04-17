using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;

namespace WebBanMyPham.Areas.Admin.Controllers
{
    public class BrandAdminController : Controller
    {
        // GET: Admin/BrandAdmin
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstBrand = new List<Brand>();
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
                lstBrand = objWebBanMyPhamEntities.Brand.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstBrand = objWebBanMyPhamEntities.Brand.Where(n => n.ShowOnHomePage == true).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {
         
            if (ModelState.IsValid)
            {
                try
                {
                    if (objBrand.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                        string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        objBrand.Avatar = fileName;
                        objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/Brand/"), fileName));
                    }
                    objBrand.CreatedOnUtc = DateTime.Now;
                    objWebBanMyPhamEntities.Brand.Add(objBrand);
                    objWebBanMyPhamEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var lstBrand = objWebBanMyPhamEntities.Brand.Where(n => n.Id == id).FirstOrDefault();
            return View(lstBrand);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = objWebBanMyPhamEntities.Brand.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objPro)
        {
            var objBrand = objWebBanMyPhamEntities.Brand.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objWebBanMyPhamEntities.Brand.Remove(objBrand);
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");

        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBrand = objWebBanMyPhamEntities.Brand.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Edit(int id, Brand objBrand)
        {
            
            if (objBrand.ImageUpload != null)
            {
                String fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                String extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objBrand.Avatar = fileName;
                objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/Brand/"), fileName));

            }
            objWebBanMyPhamEntities.Entry(objBrand).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {

            var objBrand = objWebBanMyPhamEntities.Brand.Where(n => n.ShowOnHomePage == false).ToList();
            return View("Trash", objBrand);
        }
        public ActionResult Destroy(int? id, Brand objBra)
        {
            //string ShowOnHomePage = "All";
            var objBrand = objWebBanMyPhamEntities.Brand.Where(n => n.Id == objBra.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
            objBrand.ShowOnHomePage = false;

            objBrand.CreatedOnUtc = DateTime.Now;
            objBrand.UpdatedOnUtc = DateTime.Now;

            // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objBrand).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Index");
        }

        public ActionResult Restore(int? id, Brand objBra)
        {
            //string ShowOnHomePage = "All";
            var objBrand = objWebBanMyPhamEntities.Brand.Where(n => n.Id == objBra.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
            objBrand.ShowOnHomePage = true;

            objBrand.CreatedOnUtc = DateTime.Now;
            objBrand.UpdatedOnUtc = DateTime.Now;

            // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objBrand).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Trash", "BrandAdmin");
        }
    }
}