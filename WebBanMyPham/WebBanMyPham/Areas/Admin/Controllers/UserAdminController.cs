using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBanMyPham.Context;

namespace WebBanMyPham.Areas.Admin.Controllers
{
    public class UserAdminController : Controller
    {
        // GET: Admin/UserAdmin
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstUser = new List<User>();
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
                lstUser = objWebBanMyPhamEntities.User.Where(n => n.FirstName.Contains(SearchString)).ToList();
                lstUser = objWebBanMyPhamEntities.User.Where(n => n.LastName.Contains(SearchString)).ToList();
            }
            else
            {
                lstUser = objWebBanMyPhamEntities.User.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstUser = lstUser.OrderByDescending(n => n.Id).ToList();
            return View(lstUser.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Context.User _users)
        {

            if (ModelState.IsValid)
            {

                var check = objWebBanMyPhamEntities.User.FirstOrDefault(s => s.Email == _users.Email);
                if (check == null)
                {
                    _users.Password = GetMD5(_users.Password);
                    objWebBanMyPhamEntities.Configuration.ValidateOnSaveEnabled = false;
                    objWebBanMyPhamEntities.User.Add(_users);
                    objWebBanMyPhamEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email không tồn tại";
                    return View();
                }
            }
            return View("Index");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2string = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2string += targetData[i].ToString("x2");
            }
            return byte2string;
        }

        public ActionResult Details(int id)
        {
            var lstUser = objWebBanMyPhamEntities.User.Where(n => n.Id == id).FirstOrDefault();
            return View(lstUser);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objUser = objWebBanMyPhamEntities.User.Where(n => n.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Delete(User objUs)
        {
            var objUser = objWebBanMyPhamEntities.User.Where(n => n.Id == objUs.Id).FirstOrDefault();
            objWebBanMyPhamEntities.User.Remove(objUser);
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objUser = objWebBanMyPhamEntities.User.Where(n => n.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Edit(int id, User objUser)
        {
            
        
               
            objWebBanMyPhamEntities.Entry(objUser).State = EntityState.Modified;
            objWebBanMyPhamEntities.SaveChanges();
            return RedirectToAction("Index");
        }


       
    }
}