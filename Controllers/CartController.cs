using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using solder.Models;
using solder.ViewModels;
using solder.Helpers;
using System.Collections.Generic;

namespace solder.Controllers
{
    public class CartController : Controller
    {
        IRepository _repo;
        public CartController(IRepository repo)
        {
            _repo = repo;
        }
        public ViewResult Index(string returnUrl)
        {
            Cart cart = SessionHepler.GetObject<Cart>(HttpContext.Session, "Cart");
            if(cart == null) return View( new Cart());
            return View (cart);
        }

        public IActionResult Add(int solderId)
        {
            Solder solder = _repo.GetAll<Solder>().FirstOrDefault(s => s.Id == solderId);

            if(solder != null)
            {
                Cart cart = CheckCart();
                cart.AddItem(solder, 1);
                SessionHepler.SetObject(HttpContext.Session, "Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int solderId)
        {
            Solder solder = _repo.GetAll<Solder>().FirstOrDefault(s => s.Id == solderId);

            if(solder != null)
            {
                Cart cart = CheckCart();
                if(cart.Lines.Count == 0) return BadRequest(); 
                cart.RemoveItem(solder);
                SessionHepler.SetObject(HttpContext.Session, "Cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveOne(int solderId)
        {
            Solder solder = _repo.GetAll<Solder>().FirstOrDefault(s => s.Id == solderId);

            if(solder != null)
            {
                Cart cart = CheckCart();
                if(cart.Lines.Count == 0) return BadRequest(); 
                cart.RemoveOne(solder, 1);
                SessionHepler.SetObject(HttpContext.Session, "Cart", cart);
            }

            return RedirectToAction("Index");
        }

        private Cart CheckCart()
        {
            Cart cart = SessionHepler.GetObject<Cart>(HttpContext.Session, "Cart");
            if(cart == null)
            {
                cart = new Cart();
            }
            return cart;
        }
    }
}