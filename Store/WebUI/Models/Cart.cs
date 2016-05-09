using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public void addItem(CartItem item)
        {
            items.Add(item);
        }
        public List<CartItem> getCart()
        {
            return items;
        }
        public void removeItem(CartItem item)
        {
            items.Remove(item);
        }
        public Decimal getTotal()
        {
            Decimal total = 0;
            foreach(CartItem ci in items)
            {
                total += ci.Amount * ci.Price;
            }
            return total;
        }
        public int getAmount()
        {
            int amount = 0;
            foreach(CartItem ci in items)
            {
                amount += ci.Amount;
            }
            return amount;
        }
    }
}