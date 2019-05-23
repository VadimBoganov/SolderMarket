using System.Collections.Generic;
using System.Linq;
using solder.Models;

namespace solder.Models
{
    public class Cart
    {
        public List<CartLine> Lines {get; private set;}

        public Cart()
        {
            Lines = new List<CartLine>();
        }

        public void AddItem(Solder solder, int quantity)
        {
            CartLine line = Lines.Where(s => s.Solder.Id == solder.Id).FirstOrDefault();

            if(line == null)
            {
                Lines.Add(new CartLine
                {
                    Solder = solder,
                    Quantity = quantity
                });
            }

            else 
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveOne(Solder solder, int quantity)
        {
            CartLine line = Lines.Where(s => s.Solder.Id == solder.Id).FirstOrDefault();

            if(line != null)
            {
                line.Quantity -= quantity;
            }
            
            if(line.Quantity == 0) RemoveItem(line.Solder);

        }

        public void RemoveItem(Solder solder)
        {
            Lines.RemoveAll(s =>s.Solder.Id == solder.Id);
        }

        public decimal CalcTotalPrice()
        {
            return Lines.Sum(p => p.Solder.Price * p.Quantity);
        }

        public void Clear()
        {
            Lines.Clear();
        }

    }

    public class CartLine
    {
        public Solder Solder {get;set;}
        public int Quantity {get; set;}
    }
}