using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanMyPham.Models
{
    public partial class OrderMasterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreatedOnUtc { get; set; }
    }
}