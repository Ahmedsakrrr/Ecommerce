using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        EcommerceDbContext _dbContext = new EcommerceDbContext();
        public IActionResult Index(ProductFilter filter)
        {
            decimal discount = 50;
            var products = _dbContext.Products.AsQueryable();



            if (filter.ProductName is not null)
            {
                products = products.Where(p => p.Name.Contains(filter.ProductName));
                ViewBag.ProductName = filter.ProductName;
            }
            if (filter.MinPrice > 0)
            {
                products = products.Where(p => (p.Price - (p.Price * p.Discount / 100) >= filter.MinPrice));
                ViewBag.MinPrice = filter.MinPrice;
            }
            if (filter.MaxPrice > 0)
            {
                products = products.Where(p => (p.Price - (p.Price * p.Discount / 100) <= filter.MaxPrice));
                ViewBag.MaxPrice = filter.MaxPrice;
            }
            if (filter.CategoryId > 0)
            {
                products = products.Where(p => p.CategoryId == filter.CategoryId);
                ViewBag.CategoryId = filter.CategoryId;
            }
            if (filter.BrandId > 0)
            {
                products = products.Where(p => p.BrandId == filter.BrandId);
                ViewBag.BrandId = filter.BrandId;
            }
            if (filter.IsHot)
            {
                products = products.Where(p => p.Discount > discount);
                ViewBag.IsHot = filter.IsHot;
            }
            if (filter.Page > 0)
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)products.Count() / 8);
                products = products.Skip((filter.Page - 1) * 8).Take(8);
            }


            ViewBag.Categories = _dbContext.Categories.ToList();
            ViewBag.Brands = _dbContext.Brands.ToList();

            return View(products.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {

            ViewBag.Categories = _dbContext.Categories.ToList();
            ViewBag.products = _dbContext.Products.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _dbContext.Categories.ToList();
                ViewBag.Brands = _dbContext.Brands.ToList();

                return View(product);
            }

            if (file is not null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + " _ " + file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                product.MainImg = fileName;
            }
           

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id, IFormFile file)
        {
            ViewBag.Categories = _dbContext.Categories.ToList();
            ViewBag.Brands = _dbContext.Brands.ToList();
            var product = _dbContext.Products.FirstOrDefault(b => b.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);

        }
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile file)
        {
            var productInDb = _dbContext.Products.AsNoTracking().FirstOrDefault(b => b.Id == product.Id);

            if (productInDb == null)
                return NotFound();

            if (file is not null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", productInDb.MainImg);

                product.MainImg = fileName;

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
            else
            {
                product.MainImg = productInDb.MainImg;
            }

            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id, IFormFile file)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var OldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", product.MainImg);

            if (System.IO.File.Exists(OldPath))
            {
                System.IO.File.Delete(OldPath);
            }
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
