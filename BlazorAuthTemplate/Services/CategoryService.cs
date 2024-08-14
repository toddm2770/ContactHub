using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BlazorAuthTemplate.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        private readonly IEmailSender _emailSender;
		public CategoryService(ICategoryRepository repository, IEmailSender emailSender)
		{
			_repository = repository;
            _emailSender = emailSender;
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

        public async Task UpdateCategoryAsync(CategoryDTO category, string userId)
        {
            Category? categoryToUpdate = await _repository.GetCategoryByIdAsync(category.Id, userId);
            if (categoryToUpdate != null)
            {
                categoryToUpdate.Contacts.Clear();

                categoryToUpdate.Name = category.Name;
                await _repository.UpdateCategoryAsync(categoryToUpdate, userId);
            }
        }

        public async Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId)
        {
            try
            {
                Category? category = await _repository.GetCategoryByIdAsync(categoryId, userId);
                if (category == null || category.Contacts.Count < 1)
                {
                    return false;
                }

                string recipients = string.Join(";", category.Contacts.Select(c => c.Email));

                await _emailSender.SendEmailAsync(recipients, emailData.Subject!, emailData.Message!);

                return true;
                
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

    }
}
