using WareHouse.Models.Base;
using WareHouse.Models.Connections;

namespace WareHouse.Models
{
    public class Product : BaseModel
    {
        public double Price { get; set; }
        public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public ProductDetails? Details { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public List<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();


        //[Newtonsofr.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<StorageProduct> StorageProducts { get; set; } = new List<StorageProduct>();
    }
}
