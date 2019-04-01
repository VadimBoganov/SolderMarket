using Microsoft.AspNetCore.Mvc;
using solder.Models;

namespace solder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        public HomeController(IRepository repo)
        {
            _repository = repo;
        }

        public IActionResult Index() => View();
        
    }    
}
