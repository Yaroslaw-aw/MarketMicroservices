using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WareHouse.DTO.CategoryDto;
using WareHouse.Models;

namespace WareHouse.Repositories.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly WareHouseContext context;
        private readonly IMapper mapper;

        public CategoryRepository(WareHouseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync()
        {
            List<GetCategoryDto> categoriesDto = new List<GetCategoryDto>();

            try
            {
                using (context)
                {
                    List<Category>? categories = await context.Categories.AsNoTracking().ToListAsync();
                    categoriesDto = mapper.Map(categories, categoriesDto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return categoriesDto;
        }

        public async Task<Guid?> AddCategoryAsync(CategoryDto categoryDto)
        {
            using (context)
            {
                Category? existingCategory = await context.Categories.FirstOrDefaultAsync(ec => ec.Name == categoryDto.Name);

                if (existingCategory != null) return existingCategory.Id;

                Category newCategory = mapper.Map<Category>(categoryDto);
                context.Categories.Add(newCategory);
                context.SaveChanges();
                return newCategory.Id;
            }
        }
        public async Task<Guid?> DeleteCategoryAsync(Guid? categoryId)
        {
            using(context)
            {
                Category? existingCategory = await context.Categories.FirstOrDefaultAsync(ec => ec.Id == categoryId);
                if (existingCategory is null) return null;
                context.Remove(existingCategory);
                context.SaveChanges();
                return existingCategory.Id;
            }
        }

        public async Task<Guid?> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            using (context)
            {
                Category? existingCategory = await context.Categories.FirstOrDefaultAsync(ec => ec.Id == updateCategoryDto.Id);
                if (existingCategory is null) return null;
                mapper.Map(updateCategoryDto, existingCategory);
                context.SaveChanges();
                return existingCategory.Id;
            }
        }
    }
}
