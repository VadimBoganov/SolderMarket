using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using solder.Models;
using solder.ViewModels;

namespace solder.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        public HomeController(IRepository repo)
        {
            _repository = repo;
        }

        public IActionResult Index(string name)
        {
            if(string.IsNullOrEmpty(name))
                name = string.Empty;

            SolderListViewModel slvm = new SolderListViewModel
            {
                Solders = _repository.GetAll<Solder>().Where(s => s.Name.Contains(name)).ToList(),
                Name = name
            };

            return View(slvm);
        }
        
    }    
}
