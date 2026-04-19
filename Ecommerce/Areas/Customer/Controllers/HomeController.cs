using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        EcommerceDbContext _context= new EcommerceDbContext();
        public IActionResult Index(ProductFilter filter)
        {
             decimal discount = 50;
            var products = _context.Products.AsQueryable();
            if(filter.ProductName is not null)
            {
                products = products.Where(p => p.Name.Contains(filter.ProductName));
                ViewBag.ProductName = filter.ProductName;
            }
            if (filter.MinPrice >0)
            {
                products = products.Where(p => (p.Price-(p.Price*p.Discount/100)>=filter.MinPrice));
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


            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();

            return View(products.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
