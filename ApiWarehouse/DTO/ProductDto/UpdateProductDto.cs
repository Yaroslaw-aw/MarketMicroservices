namespace WareHouse.DTO.ProductDto
{
    public class UpdateProductDto
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
