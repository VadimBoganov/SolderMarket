using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using solder.Models;
using solder.ViewModels;

namespace solder.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;

        public HomeController(IRepository r)
        {
            _repository = r;
        }

        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        public IActionResult Create() => View();
        
        [HttpPost]
        public async Task<IActionResult> Create(SolderViewModel svm)
        {
            Solder solder = new Solder() { Name = svm.Name, Type = svm.Type, Price = svm.Price };
            if(svm.Avatar != null)
            {
                byte[] imageData = null;

                using(var binaryReader = new BinaryReader(svm.Avatar.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)svm.Avatar.Length);
                }
                solder.Picture = imageData;
            }
            await _repository.AddAsync(solder);
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            Solder solder = await _repository.GetAsync(id.Value);

            if(solder != null)
                return View(solder);

            return NotFound();
        }

        // подумать по поводу згрузки картинки 
        public async Task<IActionResult> Edit(int? id)
        {
            Solder solder = await _repository.GetAsync(id.Value);

            if(solder != null)
                return View(solder);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SolderViewModel svm, int? id)
        {
            Solder solder = await _repository.GetAsync(id.Value);
            solder.Name = svm.Name;
            solder.Price = svm.Price;
            solder.Type = svm.Type;
            
            if(svm.Avatar != null)
            {
                byte[] imageData = null;

                using(var binaryReader = new BinaryReader(svm.Avatar.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)svm.Avatar.Length);
                }
                solder.Picture = imageData;
            }

            await _repository.UpdateAsync(solder);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfimDelete(int? id)
        {
            if(id != null)
            {
                Solder solder = await _repository.GetAsync(id);
                if(solder != null)
                    return View(solder);
            }
            return NotFound();
        }

        // подумать о модальном окне при выборе картинки(код должен быть один как и в Details)
        public async Task<IActionResult> Delete(int? id)
        {
            Solder solder = await _repository.GetAsync(id);
            if(solder != null)
            {
                await _repository.DeleteAsync(solder);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}