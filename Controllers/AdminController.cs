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
            ViewBag.Product = new SelectList(_repository.GetAll<Product>(), "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSolder(SolderViewModel svm)
        {
            if(ModelState.IsValid)
            {
                Solder solder = new Solder() { Name = svm.Name, SolderType = svm.SolderType, Price = svm.Price, Product = svm.Product };
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

        public IActionResult CreateProduct() => View();

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product prod)
        {
            if(ModelState.IsValid)
            {
                Product p = new Product { Id = prod.Id, Name = prod.Name};
                if(p == null)
                    return BadRequest();

                await _repository.AddAsync<Product>(p);
                return RedirectToAction("Index");    
            }
            return View(prod);
        }

        public async Task<IActionResult> DetailsSolder(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            Solder solder = await _repository.GetAsync<Solder>(id.Value);

            if(solder == null)
                return BadRequest();

            return View(solder);
        }

        public async Task<IActionResult> DetailsSolderType(int? id)
        {
            if(!id.HasValue)
               return NotFound();
            
            SolderType type = await _repository.GetAsync<SolderType>(id.Value);

            if(type == null)
                return BadRequest();

            return View(type);
        }

        public async Task<IActionResult> DetailsProduct(int? id)
        {
            if(!id.HasValue)
               return NotFound();
            
            Product p = await _repository.GetAsync<Product>(id.Value);

            if(p == null)
                return BadRequest();

            return View(p);
        }

        public async Task<IActionResult> EditSolder(int? id)
        {
            ViewBag.SolderType = new SelectList(_repository.GetAll<SolderType>(), "Id", "Name");
            ViewBag.Product = new SelectList(_repository.GetAll<Product>(), "Id", "Name");

            if(!id.HasValue)
                return NotFound();

            Solder solder = await _repository.GetAsync<Solder>(id.Value);

            if(solder == null)
                return BadRequest();

            return View(solder);
        }

        [HttpPost]
        public async Task<IActionResult> EditSolder(SolderViewModel svm, int? id)
        {
            if(!id.HasValue)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            Solder solder = await _repository.GetAsync<Solder>(id.Value);
            solder.Name = svm.Name;
            solder.Price = svm.Price;
            solder.SolderType = svm.SolderType;
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

        public async Task<IActionResult> EditSolderType(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            SolderType type = await _repository.GetAsync<SolderType>(id.Value);

            if(type == null)
                return BadRequest();

            return View(type);
        }

        [HttpPost]
        public async Task<IActionResult> EditSolderType(SolderType sType, int? id)
        {
            if(!id.HasValue)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            SolderType type = await _repository.GetAsync<SolderType>(id.Value);
            type.Name = sType.Name;
            
            await _repository.UpdateAsync<SolderType>(type);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditProduct(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            Product p = await _repository.GetAsync<Product>(id.Value);

            if(p == null)
                return BadRequest();

            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product prod, int? id)
        {
            if(!id.HasValue)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            Product p = await _repository.GetAsync<Product>(id.Value);
            p.Name = prod.Name;
            
            await _repository.UpdateAsync<Product>(p);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("DeleteSolder")]
        public async Task<IActionResult> ConfimDeleteSolder(int? id)
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
        public async Task<IActionResult> DeleteSolder(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            Solder solder = await _repository.GetAsync<Solder>(id);
            
            if(solder == null)
                return BadRequest();

            await _repository.DeleteAsync<Solder>(solder);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("DeleteSolderType")]
        public async Task<IActionResult> ConfimDeleteSolderType(int? id)
        {
            if(id != null)
            {
                SolderType type = await _repository.GetAsync<SolderType>(id);
                if(type != null)
                    return View(type);
            }
            return NotFound();
        }

        public async Task<IActionResult> DeleteSolderType(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            SolderType type = await _repository.GetAsync<SolderType>(id);
            
            if(type == null)
                return BadRequest();

            await _repository.DeleteAsync<SolderType>(type);
            return RedirectToAction("Index");    
        }

        [HttpGet]
        [ActionName("DeleteProduct")]
        public async Task<IActionResult> ConfimDeleteProduct(int? id)
        {
            if(id != null)
            {
                Product p = await _repository.GetAsync<Product>(id);
                if(p != null)
                    return View(p);
            }
            return NotFound();
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            Product p = await _repository.GetAsync<Product>(id);
            
            if(p == null)
                return BadRequest();

            await _repository.DeleteAsync<Product>(p);
            return RedirectToAction("Index");    
        }
    }
}