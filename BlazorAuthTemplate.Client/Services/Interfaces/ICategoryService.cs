using BlazorAuthTemplate.Client.Models;


namespace BlazorAuthTemplate.Client.Services.Interfaces
{
    public interface ICategoryService
    {
        //create a category for the user
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId);

        //get a list of categories for the user
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId);

		//delete Category for the user
		Task DeleteCategoryAsync(int id, string userId);
	}
}
