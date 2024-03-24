using System.ComponentModel.DataAnnotations;

namespace WareHouse.DTO.ProductDto
{
    public class ProductDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }


        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid StorageId { get; set; }
    }
}
