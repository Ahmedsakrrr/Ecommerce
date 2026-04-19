using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    //ProductSubImages(ProductId, Img)
    [PrimaryKey(nameof(ProductId), nameof(Img))]
    public class ProductSubImage
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }
        
        public string Img { get; set; }
    }
}
