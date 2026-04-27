using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //EcommerceDbContext _dbContext = new EcommerceDbContext();
        Repositories<Category> _categoryrepositoriy= new Repositories<Category>();
        public async Task<IActionResult> Index(CategoryVM Filter)
        {
            var categories = await _categoryrepositoriy.Getasync();

            if (Filter.CategoryName is not null)
            {
                categories = await _categoryrepositoriy.Getasync(
                    filter: c => c.Name.Contains(Filter.CategoryName)
                );

                ViewBag.SearchTerm = Filter.CategoryName;
            }

            // Pagination
            ViewBag.totalPages = (int)Math.Ceiling(categories.Count() / 4.0);
            ViewBag.currentPage = Filter.Page;

            categories = categories
                .Skip((Filter.Page - 1) * 4)
                .Take(4);

            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            //_dbContext.Categories.Add(category);
            //_dbContext.SaveChanges();
             await _categoryrepositoriy.CreateAsync(category);
            await _categoryrepositoriy.SaveAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            var category = await _categoryrepositoriy.GetOneAsync(filter: c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            //_dbContext.Categories.Update(category);
            //_dbContext.SaveChanges();
            _categoryrepositoriy.Update(category);
            await _categoryrepositoriy.SaveAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            //var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            var category = await _categoryrepositoriy.GetOneAsync(filter: c => c.Id == id);   
            if (category == null)
            {
                return RedirectToAction("NotFoundPage","Home");
            }
            //_dbContext.Categories.Remove(category);
            //_dbContext.SaveChanges();
            _categoryrepositoriy.Delete(category);
              await _categoryrepositoriy.SaveAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
