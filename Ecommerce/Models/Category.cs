using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace Ecommerce.Models
{
    public class Category
    {
        // Category(Id, Name, Description, Status)
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Statu { get; set; }
    }
}
