﻿using BlazorAuthTemplate.Client.Models;
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

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
		{
			try
			{
				IEnumerable<CategoryDTO> categories = await _categoryService.GetCategoriesAsync(_userId);
				return Ok(categories);
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CategoryDTO?>> GetCategoryById([FromRoute] int id)
		{
			try
			{
				CategoryDTO? category = await _categoryService.GetCategoryByIdAsync(id, _userId);
				return category == null ? NotFound() : Ok(category);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPost]
		public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO category)
		{
			try
			{
				CategoryDTO createdCategory = await _categoryService.CreateCategoryAsync(category, _userId);
				return CreatedAtAction(nameof(GetCategoryById), new {id = createdCategory.Id}, category);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCategory([FromRoute] int id)
		{
			try
			{
				await _categoryService.DeleteCategoryAsync(id, _userId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryDTO category)
		{
			if (id != category.Id)
			{
				return BadRequest();
			}

			try
			{
				await _categoryService.UpdateCategoryAsync(category, _userId);
				return Ok();
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPost("{id:int}/email")]
		public async Task<ActionResult> EmailCategory([FromRoute] int id, [FromBody] EmailData emailData)
		{
			try
			{
				bool success = await _categoryService.EmailCategoryAsync(id, emailData, _userId);

				return success ? Ok() : BadRequest();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}
	}
}
