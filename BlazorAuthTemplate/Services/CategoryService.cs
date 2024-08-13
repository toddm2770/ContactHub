using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;

namespace BlazorAuthTemplate.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
		public CategoryService(ICategoryRepository repository)
		{
			_repository = repository;
		}

		public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId)
        {
            Category? category = await _repository.GetCategoryByIdAsync(categoryId, userId); ;
            return category?.ToDTO();
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                AppUserId = userId
            };

            Category createdCategory = await _repository.CreateCategoryAsync(newCategory);

            return createdCategory.ToDTO();
        }

		public async Task DeleteCategoryAsync(int id, string userId)
		{ 
                await _repository.DeleteCategoryAsync(id, userId);  
		}

		public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            IEnumerable<Category> categories = await _repository.GetCategoriesAsync(userId);
            IEnumerable<CategoryDTO> categoryDTOs = categories.Select(c => c.ToDTO());

            return categoryDTOs;
        }

    }
}
