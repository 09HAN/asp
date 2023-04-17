using DocuSign.eSign.Model;
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
    public class OrderAdminController : Controller
    {
        // GET: Admin/OrderAdmin
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstOrder = new List<Order>();
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
                lstOrder = objWebBanMyPhamEntities.Order.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstOrder = objWebBanMyPhamEntities.Order.Where(n => n.Status == 1).ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstOrder = lstOrder.OrderByDescending(n => n.Id).ToList();
            return View(lstOrder.ToPagedList(pageNumber, pageSize));

        }

        [HttpGet]
        public ActionResult Create()
        {
        
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Order objOrder)
        {
           
            if (ModelState.IsValid)
            {

                objOrder.CreatedOnUtc = DateTime.Now;
                objWebBanMyPhamEntities.Order.Add(objOrder);
                objWebBanMyPhamEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(objOrder);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var lstOrder = objWebBanMyPhamEntities.Order.Where(n => n.Id == id).FirstOrDefault();
            return View(lstOrder);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var lstOrder = objWebBanMyPhamEntities.Order.Where(n => n.Id == id).FirstOrDefault();
            return View(lstOrder);
        }

        [HttpPost]
        public ActionResult Delete(Product objOrd)
        {
            var lstOrder = objWebBanMyPhamEntities.Order.Where(n => n.Id == objOrd.Id).FirstOrDefault();
            objWebBanMyPhamEntities.Order.Remove(lstOrder);
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Trash");

        }

        public ActionResult Trash()
        {

            var objOrder = objWebBanMyPhamEntities.Order.Where(n => n.Status == 0).ToList();
            return View("Trash", objOrder);
        }
        public ActionResult Destroy(int? id, Order objOrd)
        {
            //string ShowOnHomePage = "All";
            var objOrder = objWebBanMyPhamEntities.Order.Where(n => n.Id == objOrd.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
            objOrder.Status=0;

            objOrder.CreatedOnUtc = DateTime.Now;

            // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objOrder).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            
            var objOrder = objWebBanMyPhamEntities.Order.Where(n => n.Id == id).FirstOrDefault();
            return View(objOrder);
        }

        [HttpPost]
        public ActionResult Edit(int id, Order objOrder)
        {
           
            
            objWebBanMyPhamEntities.Entry(objOrder).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Restore(int? id, Order objOrd)
        {
            //string ShowOnHomePage = "All";
            var objOrder = objWebBanMyPhamEntities.Order.Where(n => n.Id == objOrd.Id).FirstOrDefault();
            // Product objProduct = objWebBanMyPhamEntities.Product.Find(id);
            objOrder.Status = 1;

            objOrder.CreatedOnUtc = DateTime.Now;

            // productdao.Update(objProduct);
            //objWebBanMyPhamEntities.Product.Remove(objProduct);
            objWebBanMyPhamEntities.Entry(objOrder).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            //    TempData["Thông báo"] = "Thành công";
            return RedirectToAction("Trash", "OrderAdmin");
        }
    }
}