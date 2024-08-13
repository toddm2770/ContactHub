using BlazorAuthTemplate.Models;

namespace BlazorAuthTemplate.Services.Interfaces
{
    public interface ICategoryRepository
    {
        //create
        Task<Category> CreateCategoryAsync(Category category);

        //read
        Task<Category?> GetCategoryByIdAsync(int Id, string userId);

        Task<List<Category>> GetCategoriesAsync(string userId);

        //update
        Task UpdateCategoryAsync(Category category, string userId);

        //delete
        Task DeleteCategoryAsync(int id, string userId);

    }
}
