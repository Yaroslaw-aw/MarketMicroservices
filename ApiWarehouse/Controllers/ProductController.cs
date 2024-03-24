using AutoMapper;
using Market.DTO.Caching;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WareHouse.DTO.ProductDto;
using WareHouse.Models;
using WareHouse.Repositories.ProductRepo;

namespace WareHouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        private readonly Redis redis;
        private readonly IMapper mapper;

        public ProductController(IProductRepository repository, Redis redis, IMapper mapper)
        {
            this.repository = repository;
            this.redis = redis;
            this.mapper = mapper;
        }

        [HttpGet(template: "GetAllProducts")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAllProducts()
        {
            if (redis.TryGetValue("products", out List<GetProductDto>? productsCache) && productsCache != null)
                return AcceptedAtAction(nameof(GetAllProducts), productsCache);

            IEnumerable<GetProductDto> products = await repository.GetAllProductsAsync();

            redis.SetData("products", products);

            return AcceptedAtAction(nameof(GetAllProducts), products);
        }

        [HttpPost(template: "AddProduct")]
        public async Task<ActionResult<Guid?>> AddProduct(ProductDto productDto)
        {
            Guid? newProductId = await repository.AddProductAsync(productDto);

            if (newProductId == null) return BadRequest("One field is null!");

            await redis.cache.RemoveAsync("products");

            return CreatedAtAction(nameof(AddProduct), newProductId);
        }

        [HttpDelete(template: "DeleteProduct")]
        public async Task<ActionResult<Guid?>> DeleteProduct([FromBody] Guid deleteProductId)
        {
            Guid? deletedProductId = await repository.DeleteProductAsync(deleteProductId);

            if (deletedProductId == null) return NotFound();

            await redis.cache.RemoveAsync("products");

            return AcceptedAtAction(nameof(DeleteProduct), deletedProductId);
        }

        [HttpPut(template: "UpdateProduct")]
        public async Task<ActionResult<Guid?>> UpdateProduct(UpdateProductDto updateProductDto)
        {
            Guid? updatedProductId = await repository.UpdateProductAsync(updateProductDto);

            if (updatedProductId == null) return NotFound();

            await redis.cache.RemoveAsync("products");

            return AcceptedAtAction(nameof(UpdateProduct), updatedProductId);
        }


        [HttpGet(template: "ExistsProduct")]
        public async Task<ActionResult<bool>> ExistsProduct(Guid existingProductId)
        {
            bool isProductExists = await repository.ExistsProduct(existingProductId);

            return Accepted(nameof(ExistsProduct), isProductExists);
        }


        [HttpPost(template: "AddCategoryToProduct")]
        public async Task<ActionResult<Guid?>> AddCategoryToProduct(AddCategotyToProductDto categotyToProductDto)
        {
            Guid? addedCategory = await repository.AddCategoryToProductAsync(categotyToProductDto);
            if (addedCategory == null) return StatusCode(409);
            await redis.cache.RemoveAsync("products");
            return CreatedAtAction(nameof(AddCategoryToProduct), addedCategory);
        }

        private async Task<string> GetCsvAsync(IEnumerable<Product>? products)
        {            
            StringBuilder sb = new StringBuilder();

            if (products != null)
                foreach (var line in products)
                {
                    sb.AppendLine(line.Name + ";" +
                                  line.Description + ";" +
                                  line.Price + ";");
                }
            return sb.ToString();
        }

        [HttpGet(template: "GetProductsCSV")]
        public async Task<FileContentResult> GetProductsCsv()
        {
            string? content = string.Empty;
            if (redis.TryGetValue("products", out List<Product>? productsCache) && productsCache != null)
            {                
                content = await GetCsvAsync(productsCache);
            }
            else
            {
                IEnumerable<GetProductDto> productsDto = await repository.GetAllProductsAsync();

                List<Product>? products = mapper.Map(productsDto, productsCache);

                content = await GetCsvAsync(products);
            }

            return File(new UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
        }


        [HttpGet(template: "GetProductsCsvUrl")]
        public async Task<ActionResult<string>> GetProductsCsvUrl()
        {
            string? content = string.Empty;

            if (redis.TryGetValue("products", out List<Product>? productsCache) && productsCache != null)
            {
                content = await GetCsvAsync(productsCache);
            }
            else
            {
                IEnumerable<GetProductDto> productsDto = await repository.GetAllProductsAsync();

                List<Product>? products = mapper.Map(productsDto, productsCache);

                content = await GetCsvAsync(products);
            }

            string fileName = string.Empty;

            fileName = "products" + DateTime.Now.ToBinary().ToString() + ".csv";

            System.IO.File.WriteAllText(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), content);

            return "https://" + Request.Host.ToString() + "/static/" + fileName;
        }
    }
}