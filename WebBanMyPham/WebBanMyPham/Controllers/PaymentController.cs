using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanMyPham.Context;
using WebBanMyPham.Models;

namespace WebBanMyPham.Controllers
{
    
    public class PaymentController : Controller
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        // GET: Payment
        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login","Home");
            }
            else
            {
                var lstCart = (List<CartModels>)Session["cart"];
                Order objOrder=new Order();
                objOrder.Name = "DonHang" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.UserId = int.Parse(Session["Id"].ToString());
                objOrder.CreatedOnUtc= DateTime.Now;
                objOrder.Status = 1;
                objWebBanMyPhamEntities.Order.Add(objOrder);
                objWebBanMyPhamEntities.SaveChanges();

                int intOrderId = objOrder.Id;
                List<OrderDetail> lstOrderDetail=new List<OrderDetail>();

                foreach(var item in lstCart){
                    OrderDetail obj=new OrderDetail();
                    obj.Quantity= item.Quantity;
                    obj.OrderId= intOrderId;
                    obj.ProductId = item.Product.Id;
                    lstOrderDetail.Add(obj);
                }
                objWebBanMyPhamEntities.OrderDetail.AddRange(lstOrderDetail);
                objWebBanMyPhamEntities.SaveChanges();
            }
            return View();
        }
    }
}