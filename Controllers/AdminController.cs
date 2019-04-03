using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using solder.Models;
using solder.ViewModels;

namespace solder.Controllers
{
    public class AdminController : Controller
    {
        private IRepository _repository;

        public AdminController(IRepository r)
        {
            _repository = r;
        }

        public IActionResult Index()
        {
            CommonSolderViewModel csvm = new CommonSolderViewModel
            {
                Solders = _repository.GetAll<Solder>(),
                SolderTypes = _repository.GetAll<SolderType>(),
                Products = _repository.GetAll<Product>()
            };
            return View(csvm);
        } 

        public IActionResult CreateSolder()
        {
            ViewBag.SolderType = new SelectList(_repository.GetAll<SolderType>(), "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSolder(SolderViewModel svm)
        {
            if(ModelState.IsValid)
            {
                Solder solder = new Solder() { Name = svm.Name, SolderType = svm.Type, Price = svm.Price, Product = svm.Product };
                if(svm.Avatar != null)
                {
                    byte[] imageData = null;

                    using(var binaryReader = new BinaryReader(svm.Avatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)svm.Avatar.Length);
                    }
                    solder.Picture = imageData;
                }
                await _repository.AddAsync<Solder>(solder);
                
                return RedirectToAction("Index");
            }
            return View(svm);
        }

        public IActionResult CreateSolderType() => View();

        [HttpPost]
        public async Task<IActionResult> CreateSolderType(SolderType sType)
        {
            if(ModelState.IsValid)
            {
                SolderType type = new SolderType { Id = sType.Id, Name = sType.Name};
                if(type == null)
                    return BadRequest();

                await _repository.AddAsync<SolderType>(type);
                return RedirectToAction("Index");    
            }
            return View(sType);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            Solder solder = await _repository.GetAsync<Solder>(id.Value);

            if(solder == null)
                return BadRequest();

            return View(solder);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            Solder solder = await _repository.GetAsync<Solder>(id.Value);

            if(solder == null)
                return BadRequest();

            return View(solder);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SolderViewModel svm, int? id)
        {
            if(!id.HasValue)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            Solder solder = await _repository.GetAsync<Solder>(id.Value);
            solder.Name = svm.Name;
            solder.Price = svm.Price;
            solder.SolderType = svm.Type;
            solder.Product = svm.Product;
            
            if(svm.Avatar != null)
            {
                byte[] imageData = null;

                using(var binaryReader = new BinaryReader(svm.Avatar.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)svm.Avatar.Length);
                }
                solder.Picture = imageData;
            }

            await _repository.UpdateAsync<Solder>(solder);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfimDelete(int? id)
        {
            if(id != null)
            {
                Solder solder = await _repository.GetAsync<Solder>(id);
                if(solder != null)
                    return View(solder);
            }
            return NotFound();
        }

        // подумать о модальном окне при выборе картинки(код должен быть один как и в Details)
        public async Task<IActionResult> Delete(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            Solder solder = await _repository.GetAsync<Solder>(id);
            
            if(solder == null)
                return BadRequest();

            await _repository.DeleteAsync<Solder>(solder);
            return RedirectToAction("Index");
        }
    }
}