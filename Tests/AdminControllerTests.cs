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
    public class AdminControllerTests
    {
        [Fact]
        public void Index()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAll<Solder>()).Returns(GetTestSolders());
            var controller = new AdminController(mock.Object);

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
            mock.Setup(r => r.GetAsync<Solder>(testId)).ReturnsAsync(GetTestSolders().FirstOrDefault(s => s.Id == testId));
            var controller = new AdminController(mock.Object);

            var res = await controller.DetailsSolder(testId);

            var viewResult = Assert.IsType<ViewResult>(res);
            var model = Assert.IsType<Solder>(viewResult.ViewData.Model);
            Assert.Equal("123", model.Name);
            Assert.Equal(123, model.Price);
        }

        [Fact]
        public async Task CreateModelInvalidState()
        {
            var mock = new Mock<IRepository>();
            var controller = new AdminController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            SolderViewModel sol = new SolderViewModel();

            var res = await controller.CreateSolder(sol);
            
            var viewRes = Assert.IsType<ViewResult>(res);
            Assert.Equal(sol, viewRes?.Model);
        }

        [Fact]
        public async Task CreateCheckRedirect()
        {
            var mock = new Mock<IRepository>();
            var controller = new AdminController(mock.Object);
            var prod = new SolderViewModel()
            {
                Name = "dsd",
                Type = new SolderType{ Id = 1, Name = "Babbit"},
                Price = 213
            };
            var res = await controller.CreateSolder(prod);

            var redirectResult = Assert.IsType<RedirectToActionResult>(res);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task DeleteNotFound()
        {
            var mock = new Mock<IRepository>();
            var controller = new AdminController(mock.Object);

            var result = await controller.DeleteSolder(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteRedirection()
        {
            int testId = 1;
            var mock = new Mock<IRepository>();
            mock.Setup(r => r.GetAsync<Solder>(testId)).ReturnsAsync(GetTestSolders().FirstOrDefault(s => s.Id == testId));
            var controller = new AdminController(mock.Object);

            var result = await controller.DeleteSolder(testId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("Index", redirectResult.ActionName);
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