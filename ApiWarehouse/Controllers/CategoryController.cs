using Market.DTO.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WareHouse.DTO.CategoryDto;
using WareHouse.Repositories.CategoryRepo;

namespace WareHouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository repository;
        private readonly Redis redis;

        public CategoryController(ICategoryRepository repository, IDistributedCache cache)
        {
            this.repository = repository;
            this.redis = new Redis(cache);
        }

        [HttpGet(template: "GetAllCategories")]
        public async Task<ActionResult<IEnumerable<GetCategoryDto>>> GetAllCategories()
        {
            if (redis.TryGetValue("categories", out List<GetCategoryDto>? catigoriesCache) && catigoriesCache != null)
                return AcceptedAtAction(nameof(GetAllCategories), catigoriesCache);

            IEnumerable<GetCategoryDto> categories = await repository.GetAllCategoriesAsync();

            redis.SetData("categories", categories);

            return AcceptedAtAction(nameof(GetAllCategories), categories);
        }

        [HttpPost(template: "AddCategory")]
        public async Task<ActionResult<Guid?>> AddCategory(CategoryDto categoryDto)
        {
            Guid? newCategoryId = await repository.AddCategoryAsync(categoryDto);

            await redis.cache.RemoveAsync("categories");

            return AcceptedAtAction(nameof(AddCategory), newCategoryId);
        }

        [HttpDelete(template: "DeleteCategory")]
        public async Task<ActionResult<Guid?>> DeleteCategory(Guid deleteCategoryId)
        {
            Guid? deletedCategoryId = await repository.DeleteCategoryAsync(deleteCategoryId);

            if (deletedCategoryId == null) return NotFound();

            await redis.cache.RemoveAsync("categories");

            return AcceptedAtAction(nameof(DeleteCategory), deletedCategoryId);
        }

        [HttpPut(template: "UpdateCategory")]
        public async Task<ActionResult<Guid?>> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            Guid? updatedCategoryId = await repository.UpdateCategoryAsync(updateCategoryDto);

            if (updatedCategoryId == null) return NotFound();

            await redis.cache.RemoveAsync("categories");

            return AcceptedAtAction(nameof(UpdateCategory), updatedCategoryId);
        }
    }
}
