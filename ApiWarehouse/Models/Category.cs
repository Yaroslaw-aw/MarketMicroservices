using WareHouse.Models.Base;
using WareHouse.Models.Connections;

namespace WareHouse.Models
{
    public class Category : BaseModel
    {
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();


        //[Newtonsofr.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
    }
}
