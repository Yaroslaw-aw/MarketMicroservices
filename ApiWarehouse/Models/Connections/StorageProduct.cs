namespace WareHouse.Models.Connections
{
    public class StorageProduct
    {
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid StorageId { get; set; }
        public Storage? Storage { get; set; }
    }
}
