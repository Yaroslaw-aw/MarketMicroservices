using AutoMapper;
using Market.DTO.Caching;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WareHouse.DTO.ProductDto;
using WareHouse.Models;

namespace WareHouse.Repositories.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly WareHouseContext context;
        private readonly IMapper mapper;
        private readonly Redis redis;

        public ProductRepository(WareHouseContext context, IMapper mapper, Redis redis)
        {
            this.context = context;
            this.mapper = mapper;
            this.redis = redis;
        }

        public async Task<IEnumerable<GetProductDto>> GetAllProductsAsync()
        {
            List<GetProductDto>? productsDto = new List<GetProductDto>();

            try
            {
                using (context)
                {
                    List<Product>? products = await context.Products.AsNoTracking().ToListAsync();
                    productsDto = mapper.Map(products, productsDto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return productsDto;
        }

        public async Task<bool> ExistsProduct(Guid productId)
        {
            Product? existingProduct = null;

            using (context)
            {
                existingProduct = await context.Products.FirstOrDefaultAsync(product => product.Id == productId);
            }

            if (existingProduct is null)
                return false;
            else
                return true;
        }

        public async Task<Guid?> AddProductAsync(ProductDto productDto)
        {
            if (productDto?.StorageId == null || productDto?.CategoryId == null) return null;

            Guid? newProductId = null;
            try
            {
                using (context)
                {
                    #region Проверки
                    Storage? newStorage = context.Storages.FirstOrDefault(nc => nc.Id == productDto.StorageId);
                    if (newStorage is null) return null;

                    Category? newCategory = context.Categories.FirstOrDefault(nc => nc.Id == productDto.CategoryId);
                    if (newCategory is null) return null;
                    #endregion

                    Product? existingProduct = await context.Products.FirstOrDefaultAsync(ep => ep.Name == productDto.Name &&
                                                                                                ep.Description == productDto.Description);

                    if (existingProduct != null)
                    {
                        return existingProduct.Id;
                    }

                    Product newProduct = mapper.Map<Product>(productDto);

                    context.Products.Add(newProduct);

                    newProduct.Categories.Add(newCategory);

                    newProduct.Storages.Add(newStorage);

                    await context.SaveChangesAsync();

                    newProductId = newProduct.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return newProductId;
        }

        public async Task<Guid?> DeleteProductAsync(Guid productId)
        {
            using (context)
            {
                Product? deletedProduct = await context.Products.FirstOrDefaultAsync(dp => dp.Id == productId);

                if (deletedProduct is null) return null;

                context.Products.Remove(deletedProduct);

                await context.SaveChangesAsync();

                return deletedProduct?.Id;
            }
        }

        public async Task<Guid?> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            using (context)
            {
                Product? updateProduct = await context.Products.FirstOrDefaultAsync(up => up.Id == updateProductDto.ProductId);

                if (updateProduct is null) return null;

                mapper.Map(updateProductDto, updateProduct);

                await context.SaveChangesAsync();

                return updateProduct?.Id;
            }
        }

        public async Task<Guid?> AddCategoryToProductAsync(AddCategotyToProductDto categotyToProductDto)
        {
            using (context)
            {
                Product? updatedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == categotyToProductDto.ProductId);
                if (updatedProduct is null) return null;

                Category? addedCategory = await context.Categories.FirstOrDefaultAsync(cat => cat.Id == categotyToProductDto.CategoryId);
                if (addedCategory is null) return null;
                updatedProduct.Categories.Add(addedCategory);

                context.SaveChanges();
                return addedCategory.Id;
            }
        }
    }
}