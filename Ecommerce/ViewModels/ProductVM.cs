namespace Ecommerce.ViewModels
{
    public class ProductVM
    {
        public List<Category>? Categories { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<ProductSubImage>? ProductSubImage { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
        public Product? Product { get; set; }
    }
}
