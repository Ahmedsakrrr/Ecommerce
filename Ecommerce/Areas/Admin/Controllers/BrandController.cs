
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        
        //EcommerceDbContext _Dbcontext= new EcommerceDbContext();
        Repositories<Brand> _brandrepositories = new Repositories<Brand>();
        public async Task<IActionResult> Index(BrandVM Filter )
        {
            //var brands = _Dbcontext.Brands.AsQueryable();
            var brands = await _brandrepositories.Getasync();
            if (Filter.BrandName is not null)
            {
                //brands = brands.Where(b => b.Name.Contains(Filter.BrandName));
                brands = await _brandrepositories.Getasync(
                    filter:b => b.Name.Contains(Filter.BrandName)
                    );
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
        public async Task<IActionResult> Create(Brand brand, IFormFile file)
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

            //_Dbcontext.Brands.Add(brand);
            //_Dbcontext.SaveChanges();
           await _brandrepositories.CreateAsync(brand);
            await _brandrepositories.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, IFormFile file) 
        {
            //var brand = _Dbcontext.Brands.FirstOrDefault(b => b.Id == id);
            var brand =await _brandrepositories.GetOneAsync(filter: b => b.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand, IFormFile file)
        {
            //var BrandInDb=_Dbcontext.Brands.AsNoTracking().FirstOrDefault(b => b.Id == brand.Id);
            var BrandInDb = await _brandrepositories.GetOneAsync(filter: b => b.Id == brand.Id, tracking: false);
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
           
            //_Dbcontext.Brands.Update(brand);
            //_Dbcontext.SaveChanges();
             _brandrepositories.Update(brand);
            await _brandrepositories.SaveAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id, IFormFile file)
        {
            //var brand = _Dbcontext.Brands.FirstOrDefault(b => b.Id == id);
            var brand = await _brandrepositories.GetOneAsync(b => b.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            var OldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", brand.Img);
            
            if (System.IO.File.Exists(OldPath))
            {
                System.IO.File.Delete(OldPath);
            }
            //_Dbcontext.Brands.Remove(brand);
            //_Dbcontext.SaveChanges();
            _brandrepositories.Delete(brand);
            await _brandrepositories.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
