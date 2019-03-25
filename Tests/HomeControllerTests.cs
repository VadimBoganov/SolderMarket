using solder.Controllers;
using Xunit;
using Moq;
using solder.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using solder.ViewModels;

namespace solder.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestSolders());
            var controller = new HomeController(mock.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Solder>>(viewResult.Model);
            Assert.Equal(GetTestSolders().Count, model.Count());
        }

        [Fact]
        public async Task Details()
        {
            int testId = 1;
            var mock = new Mock<IRepository>();
            mock.Setup(r => r.GetAsync(testId)).ReturnsAsync(GetTestSolders().FirstOrDefault(s => s.Id == testId));
            var controller = new HomeController(mock.Object);

            var res = await controller.Details(testId);

            var viewResult = Assert.IsType<ViewResult>(res);
            var model = Assert.IsType<Solder>(viewResult.ViewData.Model);
            Assert.Equal("123", model.Name);
            Assert.Equal(123, model.Price);
        }

        [Fact]
        public async Task CreateCheckRedirect()
        {
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            var svm = new SolderViewModel()
            {
                Name = "dsd",
                Type = SolderType.Babbit,
                Price = 213
            };

            var res = await controller.Create(svm);

            var redirectResult = Assert.IsType<RedirectToActionResult>(res);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
        }


        private List<Solder> GetTestSolders()
        {
            var solders = new List<Solder>
            {
                new Solder {Id = 1, Name = "123", Type = SolderType.Babbit, Price = 123},
                new Solder {Id = 2, Name = "32", Type = SolderType.SpecialAndFusible, Price = 122}
            };
            return solders;
        }
    }
}