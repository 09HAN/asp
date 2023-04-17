using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WebBanMyPham.Context;
using WebBanMyPham.Models;

namespace WebBanMyPham.Controllers
{
    public class HomeController : Controller
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objWebBanMyPhamEntities.Category.ToList();
            objHomeModel.ListProduct = objWebBanMyPhamEntities.Product.ToList();
            return View(objHomeModel);
        }

        [HttpGet]
        public ActionResult Register()
        {
            

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Context.User _users)
        {
            if (ModelState.IsValid)
            {
               
                var check=objWebBanMyPhamEntities.User.FirstOrDefault(s=>s.Email== _users.Email);
                if (check==null)
                {
                    _users.Password = GetMD5(_users.Password);
                    objWebBanMyPhamEntities.Configuration.ValidateOnSaveEnabled = false;
                    objWebBanMyPhamEntities.User.Add( _users );
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


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = objWebBanMyPhamEntities.User.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["Id"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        public static string GetMD5(string str)
        {
            MD5 md5=new MD5CryptoServiceProvider();
            byte[] fromData=Encoding.UTF8.GetBytes(str);
            byte[] targetData=md5.ComputeHash(fromData);
            string byte2string = null;

            for(int i=0;i<targetData.Length;i++)
            {
                byte2string += targetData[i].ToString("x2");
            }
            return byte2string;
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Context.Contact _contact)
        {

            if (ModelState.IsValid)
            {
                var check = objWebBanMyPhamEntities.Contact.FirstOrDefault(s => s.Email == _contact.Email);
                if (check == null)
                {
                    objWebBanMyPhamEntities.Configuration.ValidateOnSaveEnabled = false;
                    objWebBanMyPhamEntities.Contact.Add(_contact);
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

      
    }
}