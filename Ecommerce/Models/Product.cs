using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    //(Id, Name, Description, MainImg, Price, Quantity, Rate, Status, Discount, CategoryId, BrandId)
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainImg { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public bool Statu { get; set; }
        public decimal Discount { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Brand Brand { get; set; } 
        public int BrandId { get; set; }


    }
}
