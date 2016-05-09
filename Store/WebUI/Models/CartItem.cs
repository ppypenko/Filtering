using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class CartItem
    {
        public int CartID { get; set; }
        public string UserID { get; set; }
        public int ProductID { get; set; }
        public int Amount { get; set; }
        public Decimal Price { get; set; }
    }
}