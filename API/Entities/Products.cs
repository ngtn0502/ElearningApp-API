using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Products
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int CategoryId {get; set;}

        [Required]
        public int ImageUrl {get; set;}

        public ICollection<ProductDetail> ProductDetails { get; set; }
    }
}