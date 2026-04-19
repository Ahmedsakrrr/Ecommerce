using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        EcommerceDbContext _dbContext = new EcommerceDbContext();
        public IActionResult Index(CategoryVM Filter)
        {
            var categories = _dbContext.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(Filter.CategoryName))
            {
                categories = categories.Where(c => c.Name.Contains(Filter.CategoryName));
                ViewBag.SearchTerm = Filter.CategoryName;
            }
            //Pagination logic 
            ViewBag.totalPages = (int)Math.Ceiling(categories.Count() / 4.0);
            ViewBag.currentPage = Filter.Page;
            categories = categories.Skip((Filter.Page - 1) * 4).Take(4);

            return View(categories.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("NotFoundPage","Home");
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
