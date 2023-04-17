using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;

namespace WebBanMyPham.Areas.Admin.Controllers
{
    
    public class CategoryAdminController : Controller
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        // GET: Admin/CategoryAdmin
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstCategory = new List<Category>();
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
                lstCategory = objWebBanMyPhamEntities.Category.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstCategory = objWebBanMyPhamEntities.Category.Where(n => n.ShowOnHomePage == true).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
          
            if (ModelState.IsValid)
            {
                try
                {
                    if (objCategory.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                        string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        objCategory.Avatar = fileName;
                        objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                    }
                    objCategory.CreatedOnUtc = DateTime.Now;
                    objWebBanMyPhamEntities.Category.Add(objCategory);
                    objWebBanMyPhamEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var lstCategory = objWebBanMyPhamEntities.Category.Where(n => n.Id == id).FirstOrDefault();
            return View(lstCategory);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = objWebBanMyPhamEntities.Category.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Category objCat)
        {
            var objCategory = objWebBanMyPhamEntities.Category.Where(n => n.Id == objCat.Id).FirstOrDefault();
            objWebBanMyPhamEntities.Category.Remove(objCategory);
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");

        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objCategory = objWebBanMyPhamEntities.Category.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Edit(int id, Category objCategory)
        {
            
            if (objCategory.ImageUpload != null)
            {
                String fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                String extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objCategory.Avatar = fileName;
                objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));

            }
            objWebBanMyPhamEntities.Entry(objCategory).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Trash()
        {

            var objCategory = objWebBanMyPhamEntities.Category.Where(n => n.ShowOnHomePage == false).ToList();
            return View("Trash", objCategory);
        }
        public ActionResult Destroy(int? id, Category objCat)
        {
            //string ShowOnHomePage = "All";
            var objCategory = objWebBanMyPhamEntities.Category.Where(n => n.Id == objCat.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
            objCategory.ShowOnHomePage = false;

            objCategory.CreatedOnUtc = DateTime.Now;
            objCategory.UpdatedOnUtc = DateTime.Now;

            // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objCategory).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Index");
        }
        //  public ActionResult Trash()
        //  {

        //      return View();


        // }

        public ActionResult Restore(int? id, Category objCat)
        {
            //string ShowOnHomePage = "All";
            var objCategory = objWebBanMyPhamEntities.Category.Where(n => n.Id == objCat.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
            objCategory.ShowOnHomePage = true;

            objCategory.CreatedOnUtc = DateTime.Now;
            objCategory.UpdatedOnUtc = DateTime.Now;

            // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objCategory).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Trash", "CategoryAdmin");
        }
    }
}