using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Test;

namespace MVC_Test.Controllers
{
    public class ProductsController : Controller
    {
        private ACME_INCEntities db = new ACME_INCEntities();

        // GET: Products
        
        public ActionResult Index(string search = "")
        {


            var pRODUCTS = db.PRODUCTS.Where(p => p.PRODUCT_NAME.Contains(search));
                
                //db.PRODUCTS.Include(p => p.CATEGORy);
            


            return View(pRODUCTS.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCTS.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT);
        }

        // GET: Products/Create
        [Authorize(Roles ="Employee")]
        public ActionResult Create()
        {
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORIES, "ID", "CATEGORY_NAME");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public ActionResult Create([Bind(Include = "PRODUCT_NAME,PRODUCT_DESCRIPTION,PRICE,CATEGORY_ID")] PRODUCT pRODUCT, HttpPostedFileBase Product_Image)
        {
            if (ModelState.IsValid)
            {
                if(Product_Image != null)
                {
                    pRODUCT.PRODUCT_IMAGE = new byte[Product_Image.ContentLength];
                    Product_Image.InputStream.Read(pRODUCT.PRODUCT_IMAGE, 0, Product_Image.ContentLength);
                }
                db.PRODUCTS.Add(pRODUCT);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }

            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORIES, "ID", "CATEGORY_NAME", pRODUCT.CATEGORY_ID);
            return View(pRODUCT);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCTS.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORIES, "ID", "CATEGORY_NAME", pRODUCT.CATEGORY_ID);
            return View(pRODUCT);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]
        public ActionResult Edit([Bind(Include = "ID,PRODUCT_NAME,PRODUCT_DESCRIPTION,PRICE,CATEGORY_ID")] PRODUCT pRODUCT, HttpPostedFileBase Product_Image)
        {
            if (ModelState.IsValid)
            {
                if(Product_Image != null)
                {
                    pRODUCT.PRODUCT_IMAGE = new byte[Product_Image.ContentLength];
                    Product_Image.InputStream.Read(pRODUCT.PRODUCT_IMAGE, 0, Product_Image.ContentLength);

                }
                db.Entry(pRODUCT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CATEGORY_ID = new SelectList(db.CATEGORIES, "ID", "CATEGORY_NAME", pRODUCT.CATEGORY_ID);
            return View(pRODUCT);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCTS.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PRODUCT pRODUCT = db.PRODUCTS.Find(id);
            db.PRODUCTS.Remove(pRODUCT);
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
