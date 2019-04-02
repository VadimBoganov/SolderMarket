using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using solder.Controllers;
using solder.Models;
using solder.ViewModels;
using Xunit;

namespace solder.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAll<Solder>()).Returns(GetTestSolders());
            var controller = new HomeController(mock.Object);

            var result = controller.Index("1");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SolderListViewModel>(viewResult.Model);
            Assert.Equal(GetTestSolders().Where(s => s.Name.Contains("1")).Count(), model.Solders.Count());
        }

        private List<Solder> GetTestSolders()
        {
            var solders = new List<Solder>
            {
                new Solder {Id = 1, Name = "123", SolderType = new SolderType{Id = 1, Name = "Babbit"} , Price = 123},
                new Solder {Id = 2, Name = "32", SolderType = new SolderType{Id = 1, Name = "Babbit"}, Price = 122}
            };
            return solders;
        }
    }
}