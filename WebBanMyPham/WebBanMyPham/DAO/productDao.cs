using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using WebBanMyPham.Models;
using WebBanMyPham.Context;
using System.Data.Entity;

namespace WebBanMyPham.DAO
{
    public class productDao
    {
        WebBanMyPhamEntities objWebBanMyPhamEntities = new WebBanMyPhamEntities();
        public List<Product> getList(string ShowOnHomePage = "All")
        {
            
            if (ShowOnHomePage == "Index")
            {
                return objWebBanMyPhamEntities.Product.Where(n => n.ShowOnHomePage == true).ToList();

            }
            if (ShowOnHomePage == "Trash")
            {
                return objWebBanMyPhamEntities.Product.Where(n => n.ShowOnHomePage == false).ToList();
            }
            return objWebBanMyPhamEntities.Product.ToList();
        }

        public int Update(Product row)
        {
            objWebBanMyPhamEntities.Entry(row).State=EntityState.Modified;
            return objWebBanMyPhamEntities.SaveChanges();
        }
    }
}