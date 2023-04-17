using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanMyPham.Context;

namespace WebBanMyPham.Models
{
    public class HomeModel
    {
        public List<Category> ListCategory { get; set; }
        public List<Product> ListProduct { get; set; }
    }
}