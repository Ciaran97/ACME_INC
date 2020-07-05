using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Test;

namespace MVC_Test.Controllers
{
    public class OrdersController : Controller
    {
        private ACME_INCEntities db = new ACME_INCEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var OrderS = db.ORDERS.Include(o => o.AspNetUser);
            return View(OrderS.ToList());
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
        public ActionResult Create([Bind(Include = "ID,DATE_ORDERED,COURIER,ADDRESS,CUSTOMER_ID")] ORDER Order)
        {
            if (ModelState.IsValid)
            {
                db.ORDERS.Add(Order);
                db.SaveChanges();
                return RedirectToAction("Index");
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
    }
}
