using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebBanMyPham.Context;
using WebBanMyPham.Models;

namespace WebBanMyPham.Controllers
{
    public class ShoppingCartController : Controller
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View((List<CartModels>)Session["cart"]);
        }

        public ActionResult AddToCart(int id, int quantity)
        {
            if (Session["cart"] == null)
            {
                List<CartModels> cart = new List<CartModels> ();
                cart.Add(new CartModels { Product = objWebBanMyPhamEntities.Product.Find(id), Quantity = quantity });
                Session["cart"]=cart;
                Session["count"] = 1;
            }
            else
            {
                List<CartModels> cart = ( List<CartModels>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity += quantity;

                }
                else
                {
                    cart.Add(new CartModels { Product = objWebBanMyPhamEntities.Product.Find(id), Quantity = quantity });
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                Session["cart"] = cart;
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        private int isExist(int id)
        {
            List<CartModels> cart = (List<CartModels>)Session["cart"];
            for(int i=0;i<cart.Count;i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }

        public ActionResult ReMove(int Id)
        {
            List<CartModels> li = (List<CartModels>)Session["cart"];
            li.RemoveAll(x => x.Product.Id == Id);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) -1;
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }
    }
}