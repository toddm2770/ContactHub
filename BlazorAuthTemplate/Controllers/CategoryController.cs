using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Components.Account;
using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuthTemplate.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		private string _userId => User.GetUserId()!;


		public async Task<ActionResult<CategoryDTO>> CreateCategoryAsync(CategoryDTO category, string userId)
		{
			CategoryDTO createdCategory = await _categoryService.CreateCategoryAsync(category, userId);

			return createdCategory;
		}

		public async Task<ActionResult<CategoryDTO>> GetCategoriesAsync(string userId)
		{
			var categories = await _categoryService.GetCategoriesAsync(userId);
			return Ok(categories);
		}


		public async Task<ActionResult> DeleteCategoryAsync(int id, string userId)
		{
			await _categoryService.DeleteCategoryAsync(id, userId);
			return NoContent();
		}
	}
}
