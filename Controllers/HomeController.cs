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

        public IActionResult Index(string solderName, string typeName, string productName)
        {
            if(solderName == null)
                solderName = string.Empty;

            if(typeName == null)
                typeName = string.Empty;    

            if(productName == null)
                productName = string.Empty;        
                
            SolderListViewModel slvm = new SolderListViewModel
            {
                Solders = _repository.GetAll<Solder>()
                    .Where(s =>s.Name.Contains(solderName))
                    .Where(s => s.SolderType.Name.Contains(typeName))
                    .Where(s => s.SolderProduct.Name.Contains(productName)),
                SolderTypes = _repository.GetAll<SolderType>(),
                SolderProducts = _repository.GetAll<SolderProduct>(),
                Name = solderName
            };

            return View(slvm);
        }
        
    }    
}
