using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC_Test;
using MVC_Test.Models;

namespace MVC_Test.Controllers
{
    public class OrdersController : Controller
    {
        private ACME_INCEntities db = new ACME_INCEntities();

        // GET: Orders
        [Authorize(Roles = "Employees")]
        public ActionResult Index()
        {

            String UserId = User.Identity.GetUserId();

            // var Categories = db.ORDER_DETAILS.Select(OD => OD.PRODUCT.CATEGORy).Distinct();

            var tester = (from od in db.ORDER_DETAILS
                          join p in db.PRODUCTS on od.PRODUCT_ID equals p.ID
                          join c in db.CATEGORIES on p.CATEGORY_ID equals c.ID
                          group od by c.ID into g
                          select new Orders
                          {
                              CategoryName = g.FirstOrDefault().PRODUCT.CATEGORy.CATEGORY_NAME,
                              TotalPrice = (decimal)g.Sum(p => p.PRODUCT.PRICE),
                              ProductCount = g.Count()

                          });

            
            


            return View(tester.ToList());
            
            
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER Order = db.ORDERS.Find(id);
            if (Order == null)
            {
                return HttpNotFound();
            }
            return View(Order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CUSTOMER_ID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COURIER,ADDRESS,CUSTOMER_ID")] ORDER Order)
        {
            if (ModelState.IsValid)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                ORDER_DETAILS od;
                Order.CUSTOMER_ID = User.Identity.GetUserId();
                Order.DATE_ORDERED = DateTime.Now;
                db.ORDERS.Add(Order);

                foreach(Item item in cart)
                {
                    od = new ORDER_DETAILS();
                    od.ORDER_ID = db.ORDERS.Find(Order.ID).ID;
                    od.PRODUCT_ID = item.Product.ID;
                    od.QUANTITY = item.Quantity;
                    db.ORDER_DETAILS.Add(od);
                    
                }
                


                
                db.SaveChanges();
                Session["cart"] = null;
                return RedirectToAction("Index", "Success");
            }

            ViewBag.CUSTOMER_ID = new SelectList(db.AspNetUsers, "Id", "Email", Order.CUSTOMER_ID);
            return View(Order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER Order = db.ORDERS.Find(id);
            if (Order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CUSTOMER_ID = new SelectList(db.AspNetUsers, "Id", "Email", Order.CUSTOMER_ID);
            return View(Order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DATE_ORDERED,COURIER,ADDRESS,CUSTOMER_ID")] ORDER Order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CUSTOMER_ID = new SelectList(db.AspNetUsers, "Id", "Email", Order.CUSTOMER_ID);
            return View(Order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER Order = db.ORDERS.Find(id);
            if (Order == null)
            {
                return HttpNotFound();
            }
            return View(Order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ORDER Order = db.ORDERS.Find(id);
            db.ORDERS.Remove(Order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


   


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CustOrders()
        {

            String UserId = User.Identity.GetUserId();

            var OrderS = db.ORDERS.Include(o => o.AspNetUser).Distinct();
            // var Categories = db.ORDER_DETAILS.Select(OD => OD.PRODUCT.CATEGORy).Distinct();

         
                OrderS = OrderS.Where(o => o.AspNetUser.Id.Equals(UserId));
            


            return View(OrderS.ToList());


        }

        public ActionResult DrawChart()
        {
            List<String> CatNames = new List<String>();
            List<decimal> Totals = new List<decimal>();


            var tester = (from od in db.ORDER_DETAILS
                          join p in db.PRODUCTS on od.PRODUCT_ID equals p.ID
                          join c in db.CATEGORIES on p.CATEGORY_ID equals c.ID
                          group od by c.ID into g
                          select new Orders
                          {
                              CategoryName = g.FirstOrDefault().PRODUCT.CATEGORy.CATEGORY_NAME,
                              TotalPrice = (decimal)g.Sum(p => p.PRODUCT.PRICE)
                          });
           
            foreach (var item in tester)
            {
                CatNames.Add(item.CategoryName);
                Totals.Add(item.TotalPrice);
            }
            

            var chart = new Chart(width: 500, height: 400, theme: ChartTheme.Yellow)
            .AddTitle("Sales Performance Report").DataBindTable(dataSource: tester, "CategoryName").SetXAxis().Write();


            return null;
        }
    }
}
