using Microsoft.AspNetCore.Mvc;
using WareHouse.DTO.CategoryDto;

namespace WareHouse.Repositories.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync();
        Task<Guid?> AddCategoryAsync(CategoryDto categoryDto);
        Task<Guid?> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task<Guid?> DeleteCategoryAsync(Guid? categoryId);
    }
}
