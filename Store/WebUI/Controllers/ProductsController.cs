using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Entities;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private StoreMasterEntities db = new StoreMasterEntities();
        private Models.Cart cart = new Models.Cart();
        // GET: Products
        public ActionResult Index(string productCategory, string searchString)
        {
            var CategoryQry = from m in db.Products orderby m.Category select m.Category;

            var CategoryList = new List<string>();
            CategoryList.AddRange(CategoryQry.Distinct());
            ViewData["productCategory"] = new SelectList(CategoryList);

            var products = from m in db.Products select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(productCategory))
            {
                products = products.Where(x => x.Category == productCategory);
            }

            return View(products.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int id)
        {
            Product product = (Product)from p in db.Products
                        where p.ProductID == id
                        select p;
            
            string user = User.Identity.GetUserId();
            CartItem ci = new CartItem();

            return View();
        }

        //GET: Products/Cart
        public ActionResult Cart()
        {
            return View(cart);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            //---------------start tracking views---------------
            addToViewedProducts((int)id);
            ViewBag.Viewed = getViewed((int)id);
            ViewBag.Product = product;
            return View();
        }
        

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Image,Description,Category,Price,Rating")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Image,Description,Category,Price,Rating")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void addToViewedProducts(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string user = User.Identity.GetUserId();
                Boolean check = db.ViewedProducts.Where(c => c.UserID == user).Where(c => c.ProductID == id).Any();
                if (!check)
                {
                    ViewedProduct v = new ViewedProduct();
                    v.ProductID = id;
                    v.UserID = user;
                    db.ViewedProducts.Add(v);
                    db.SaveChanges();
                }
            }
        }
        private List<Product> getViewed(int id)
        {
            string user = User.Identity.GetUserId();

            List<string> userlist = db.ViewedProducts.Where(u => u.UserID != user).Where(i => i.ProductID == id).Select(u => u.UserID).ToList();
            List<int> sqlv1 = (from vp in db.ViewedProducts
                               from ul in userlist
                               where ul.Contains(vp.UserID)
                               where vp.ProductID != id
                               group vp by new { vp.ProductID } into g
                               orderby g.Count() descending
                               select g.Key.ProductID).Take(5).ToList();

            HashSet<int> ids = new HashSet<int>(sqlv1);

            List<Product> products = db.Products.Where(u => ids.Contains(u.ProductID)).ToList();

            return products;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
