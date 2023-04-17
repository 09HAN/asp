﻿using System.Web.Mvc;

namespace WebBanMyPham.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                   new { controller = "HomeAdmin", action = "Index", id = UrlParameter.Optional },
                new[] { "WebBanMyPham.Areas.Admin.Controllers" }
            );
        }
    }
}