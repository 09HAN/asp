using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WebBanMyPham.Models
{
    public partial class UserMasterData
    {
       
        public int Id { get; set; }
      //  [Display(FirstName = "Họ")]
        public string FirstName { get; set; }
     //  [Display(LastName = "Tên")]
        public string LastName { get; set; }
       
      
        public string Email { get; set; }
       
       // [Display(Password = "Họ")]
        public string Password { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
    }
}