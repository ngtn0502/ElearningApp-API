using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Product
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
        public int CategoryId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Instructor { get; set; }

        [Required]
        public string Language { get; set; }
    }
}