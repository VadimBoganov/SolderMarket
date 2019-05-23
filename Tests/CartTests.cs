using System.Collections.Generic;
using System.Linq;
using solder.Models;
using Xunit;

namespace solder.Tests
{
    public class CartTests
    {
        AdminControllerTests adminTests;
        List<Solder> solders;

        public CartTests()
        {
            adminTests = new AdminControllerTests();
            solders = adminTests.GetTestSolders();

             if(solders == null)
                throw new System.ArgumentException();
        }
        [Fact]
        public void Add_New_Item()
        {
            Cart cart = new Cart();

            cart.AddItem(solders[0], 1);
            cart.AddItem(solders[1], 1);

            Assert.Equal(2, cart.Lines.Count);
            Assert.Equal(solders[0], cart.Lines[0].Solder);
        }

        [Fact]
        public void Add_Quantity_To_Existing_Items()
        {
            Cart cart = new Cart();

            cart.AddItem(solders[0], 1);
            cart.AddItem(solders[1], 2);
            cart.AddItem(solders[0], 5);

            var lines = cart.Lines.OrderBy(s => s.Solder.Id).ToList();
            Assert.Equal(2, cart.Lines.Count);
            Assert.Equal(6, lines[0].Quantity);
            Assert.Equal(2, lines[1].Quantity);
        }

        [Fact]
        public void Remove_Item()
        {
            Cart cart = new Cart();

            cart.AddItem(solders[0], 1);
            cart.AddItem(solders[1], 2);
            cart.AddItem(solders[0], 5);

            cart.RemoveItem(solders[1]);

            Assert.Empty(cart.Lines.Where(s => s.Solder == solders[1]));
            Assert.Single(cart.Lines);
        }

        [Fact]
        public void Calc_Total()
        {
            Cart cart = new Cart();

            cart.AddItem(solders[0], 1);
            cart.AddItem(solders[1], 2);
            cart.AddItem(solders[0], 5);

            var res = cart.CalcTotalPrice();

            Assert.Equal(982, res);
        }

        [Fact]
        public void Clear()
        {
            Cart cart = new Cart();

            cart.AddItem(solders[0], 1);
            cart.AddItem(solders[1], 2);
            cart.AddItem(solders[0], 5);

            cart.Clear();

            Assert.Empty(cart.Lines);
        }
    }
}