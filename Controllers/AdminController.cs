using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private IHostingEnvironment _env;

        public AdminController(IRepository r, IHostingEnvironment e)
        {
            _repository = r;
            _env = e;
        }

        public IActionResult Index(SortState sortOrder = SortState.NameAsc)
        {
            CommonSolderViewModel _csvm = new CommonSolderViewModel
            {
                Solders = _repository.GetAll<Solder>(),
                SolderTypes = _repository.GetAll<SolderType>(),
                SolderProducts = _repository.GetAll<SolderProduct>()
            };

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    _csvm.Solders = _csvm.Solders.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriceAsc:
                    _csvm.Solders = _csvm.Solders.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                   _csvm.Solders = _csvm.Solders.OrderByDescending(s => s.Price);
                    break;
                case SortState.SolderTypeAsc:
                    _csvm.Solders = _csvm.Solders.OrderBy(s => s.SolderType.Name);
                    break;
                case SortState.SolderTypeDesc:
                    _csvm.Solders = _csvm.Solders.OrderByDescending(s => s.SolderType.Name);
                    break;
                case SortState.SolderProductAsc:
                    _csvm.Solders = _csvm.Solders.OrderBy(s => s.SolderProduct.Name);
                    break;
                case SortState.SolderProductDesc:
                    _csvm.Solders = _csvm.Solders.OrderByDescending(s => s.SolderProduct.Name);
                    break;        
                default:
                    _csvm.Solders = _csvm.Solders.OrderBy(s => s.Name);
                    break;
            }

            AdminIndexViewModel aivm = new AdminIndexViewModel
            {
                csvm = _csvm,
                svm = new SortViewModel(sortOrder)
            };

            return View(aivm);
        } 

        public IActionResult CreateSolder()
        {
            ViewBag.SolderType = new SelectList(_repository.GetAll<SolderType>(), "Id", "Name");
            ViewBag.Product = new SelectList(_repository.GetAll<SolderProduct>(), "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSolder(SolderViewModel svm)
        {
            if(ModelState.IsValid)
            {
                Solder solder = new Solder() 
                {
                    Name = svm.Name, 
                    SolderTypeId = svm.SolderTypeId,
                    SolderType = _repository.Get<SolderType>(svm.SolderTypeId), 
                    Price = svm.Price, 
                    ProductId = svm.ProductId,
                    SolderProduct = _repository.Get<SolderProduct>(svm.ProductId) 
                };

                if(svm.Avatar != null)
                {
                    solder.PictureName = svm.Avatar.FileName;

                    string path = "/images/solders/" + svm.Avatar.FileName;    
                    using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
                    {
                        await svm.Avatar.CopyToAsync(fileStream);
                    }                
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
        public async Task<IActionResult> CreateProduct(SolderProduct prod)
        {
            if(ModelState.IsValid)
            {
                SolderProduct p = new SolderProduct { Id = prod.Id, Name = prod.Name};
                if(p == null)
                    return BadRequest();

                await _repository.AddAsync<SolderProduct>(p);
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
            
            SolderProduct p = await _repository.GetAsync<SolderProduct>(id.Value);

            if(p == null)
                return BadRequest();

            return View(p);
        }

        public async Task<IActionResult> EditSolder(int? id)
        {
            ViewBag.SolderType = new SelectList(_repository.GetAll<SolderType>(), "Id", "Name");
            ViewBag.Product = new SelectList(_repository.GetAll<SolderProduct>(), "Id", "Name");

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
            solder.SolderType = _repository.Get<SolderType>(svm.SolderTypeId);
            solder.SolderProduct = _repository.Get<SolderProduct>(svm.ProductId);
            
            if(svm.Avatar != null)
            {
                string path = "/solders/" + svm.Avatar.FileName;    
                using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
                {
                    await svm.Avatar.CopyToAsync(fileStream);
                }      

                solder.PictureName = svm.Avatar.FileName;
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

            SolderProduct p = await _repository.GetAsync<SolderProduct>(id.Value);

            if(p == null)
                return BadRequest();

            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(SolderProduct prod, int? id)
        {
            if(!id.HasValue)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();

            SolderProduct p = await _repository.GetAsync<SolderProduct>(id.Value);
            p.Name = prod.Name;
            
            await _repository.UpdateAsync<SolderProduct>(p);

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

            var fullPath = _env.WebRootPath + "/images/solders/" + solder.PictureName;

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                ViewBag.deleteSuccess = "true";
            }

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
                SolderProduct p = await _repository.GetAsync<SolderProduct>(id);
                if(p != null)
                    return View(p);
            }
            return NotFound();
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            SolderProduct p = await _repository.GetAsync<SolderProduct>(id);
            
            if(p == null)
                return BadRequest();

            await _repository.DeleteAsync<SolderProduct>(p);
            return RedirectToAction("Index");    
        }
    }
}