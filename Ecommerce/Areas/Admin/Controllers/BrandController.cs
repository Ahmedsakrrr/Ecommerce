
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        
        EcommerceDbContext _Dbcontext= new EcommerceDbContext();
        public IActionResult Index(BrandVM Filter )
        {
            var brands = _Dbcontext.Brands.AsQueryable();
            if(Filter.BrandName is not null)
            {
                brands = brands.Where(b => b.Name.Contains(Filter.BrandName));
                ViewBag.Name = Filter.BrandName;
            }
            ViewBag.Totalpages = (int)Math.Ceiling((decimal)brands.Count() / 3);
            ViewBag.currentpage = Filter.Page;
            brands=brands.Skip((Filter.Page-1)*3).Take(3);
            
            return View(brands.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand, IFormFile file)
        {
            if (file is not null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + " _ " + file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                brand.Img= fileName;
            }

            _Dbcontext.Brands.Add(brand);
            _Dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id, IFormFile file) 
        {
            var brand = _Dbcontext.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);

        }
        [HttpPost]
        public IActionResult Edit(Brand brand, IFormFile file)
        {
            var BrandInDb=_Dbcontext.Brands.AsNoTracking().FirstOrDefault(b => b.Id == brand.Id);
            if (BrandInDb == null)
                return NotFound();
            if (file is not null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + " _ " + file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                var OldPath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images",BrandInDb.Img);
                brand.Img = fileName;
                 if( System.IO.File.Exists(OldPath))
                {
                    System.IO.File.Delete(OldPath);
                }
            }
            else
            {
                brand.Img = BrandInDb.Img;
            }
           
            _Dbcontext.Brands.Update(brand);
            _Dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Delete(int id, IFormFile file)
        {
            var brand = _Dbcontext.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            var OldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", brand.Img);
            
            if (System.IO.File.Exists(OldPath))
            {
                System.IO.File.Delete(OldPath);
            }
            _Dbcontext.Brands.Remove(brand);
            _Dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
