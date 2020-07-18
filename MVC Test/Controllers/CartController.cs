using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Test.Controllers
{

    [Authorize(Roles = "Customer")]
    [Authorize(Roles = "Employee")]
    public class CartController : Controller
    {

        private ACME_INCEntities db = new ACME_INCEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buy(int id)
        {
            PRODUCT productModel = new PRODUCT();
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = db.PRODUCTS.Find(id), Quantity = 1 });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = db.PRODUCTS.Find(id), Quantity = 1 });
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            
            int index = isExist(id);
            if(cart[index].Quantity > 1)
            {
                cart[index].Quantity = cart[index].Quantity - 1;
            }
            else if(cart[index].Quantity == 1)
            {
                cart.RemoveAt(index);
            }
            
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ID.Equals(id))
                {
                    return i;
                }
            }
                    return -1;
        }
    }
}