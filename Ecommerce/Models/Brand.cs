using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Brand
    {
        
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; } = "default.jpg";
        public bool Statu { get; set; }


    }
}
