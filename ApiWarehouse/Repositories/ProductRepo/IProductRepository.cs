using Microsoft.AspNetCore.Mvc;
using WareHouse.DTO.ProductDto;
using WareHouse.Models;

namespace WareHouse.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        Task<IEnumerable<GetProductDto>> GetAllProductsAsync();
        Task<Guid?> AddProductAsync(ProductDto productDro);
        Task<Guid?> DeleteProductAsync(Guid productId);
        Task<Guid?> UpdateProductAsync(UpdateProductDto updateProductDto);

        Task<Guid?> AddCategoryToProductAsync(AddCategotyToProductDto categotyToProductDto);

        Task<bool> ExistsProduct(Guid productId);
    }
}
