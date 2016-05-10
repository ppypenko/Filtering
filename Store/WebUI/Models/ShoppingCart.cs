using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ShoppingCart
    {
        private List<Cart> items = new List<Cart>();

        public void addItemToCart(Cart item)
        {
            foreach(Cart c in items)
            {
                if(c.ProductID == item.ProductID)
                {
                    c.Amount += item.Amount;
                    break;
                }else
                {
                    items.Add(item);
                    break;
                }
            }           
        }
        public List<Cart> getCart()
        {
            return items;
        }
        public void removeItemFromCart(Cart item)
        {
            items.Remove(item);
        }
        public Decimal getTotalPrice()
        {
            Decimal total = 0;
            foreach(Cart ci in items)
            {
                total += ci.Amount * ci.Price;
            }
            return total;
        }
        public int getTotalItems()
        {
            int amount = 0;
            foreach(Cart ci in items)
            {
                amount += ci.Amount;
            }
            return amount;
        }
    }
    public class CartItem
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string ID { get; set; }
        public Decimal Price { get; set; }
        public int Amount { get; set; }
    }
}