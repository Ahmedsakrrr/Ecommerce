using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        Repositories<Product> _productRepository = new Repositories<Product>();
        Repositories<Category> _categoryRepository = new Repositories<Category>();
        Repositories<Brand> _brandRepository = new Repositories<Brand>();
        public async Task<IActionResult> Index(ProductFilter filter)
        {
            decimal discount = 50;

            var products = await _productRepository.Getasync(includes: [p => p.Category, p => p.Brand]);

            if (filter.ProductName is not null)
            {

                products = await _productRepository.Getasync(filter: p => p.Name.Contains(filter.ProductName));
                ViewBag.ProductName = filter.ProductName;
            }
            if (filter.MinPrice > 0)
            {

                products = await _productRepository.Getasync(filter: p => (p.Price - (p.Price * p.Discount / 100) >= filter.MinPrice));
                ViewBag.MinPrice = filter.MinPrice;
            }
            if (filter.MaxPrice > 0)
            {

                products = await _productRepository.Getasync(filter: p => (p.Price - (p.Price * p.Discount / 100) <= filter.MaxPrice));
                ViewBag.MaxPrice = filter.MaxPrice;
            }
            if (filter.CategoryId > 0)
            {

                products = await _productRepository.Getasync(p => p.CategoryId == filter.CategoryId);
                ViewBag.CategoryId = filter.CategoryId;
            }
            if (filter.BrandId > 0)
            {

                products = await _productRepository.Getasync(p => p.BrandId == filter.BrandId);
                ViewBag.BrandId = filter.BrandId;
            }
            if (filter.IsHot)
            {

                products = await _productRepository.Getasync(p => p.Discount > discount);
                ViewBag.IsHot = filter.IsHot;
            }
            if (filter.Page > 0)
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)products.Count() / 8);
                products = products.Skip((filter.Page - 1) * 8).Take(8);
            }



            ViewBag.Categories = await _categoryRepository.Getasync();
            ViewBag.Brands = await _brandRepository.Getasync();

            return View(products.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var categories = await _categoryRepository.Getasync();
            var brands = await _brandRepository.Getasync();

            return View(new ProductVM()
            {
                Categories = categories.ToList(),
                Brands = brands.ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile file)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Categories = await _categoryRepository.Getasync();
                ViewBag.Brands = await _brandRepository.Getasync();
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

            await _productRepository.CreateAsync(product);
            await _productRepository.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, IFormFile file)
        {

            var product = await _productRepository.GetOneAsync(filter: b => b.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductVM
            {
                Categories = (await _categoryRepository.Getasync()).ToList(),
                Brands = (await _brandRepository.Getasync()).ToList(),
                Product = product

            });

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile file)
        {

            var productInDb = await _productRepository.GetOneAsync(filter: b => b.Id == product.Id, tracking: false);

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

            _productRepository.Update(product);
            await _productRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id, IFormFile file)
        {
            var product = await _productRepository.GetOneAsync(filter: p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var OldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", product.MainImg);
            if (System.IO.File.Exists(OldPath))
            {
                System.IO.File.Delete(OldPath);
            }
            _productRepository.Delete(product);
            await _productRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
