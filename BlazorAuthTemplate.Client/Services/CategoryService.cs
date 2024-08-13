﻿using BlazorAuthTemplate.Client.Models;
using System.Net.Http.Json;
using System.Net.Http;
using BlazorAuthTemplate.Client.Services.Interfaces;

namespace BlazorAuthTemplate.Client.Services
{
	public class CategoryService : ICategoryService
	{

		private readonly HttpClient _httpClient;

		public CategoryService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/category", category);
			response.EnsureSuccessStatusCode();

			CategoryDTO? categoryDTO = await response.Content.ReadFromJsonAsync<CategoryDTO>();
			return categoryDTO!;
		}

		public async Task DeleteCategoryAsync(int id, string userId)
		{
			var response = await _httpClient.DeleteAsync($"api/TaskerItem/{id}");
			response.EnsureSuccessStatusCode();
		}

		public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>($"api/category") ?? [];
		}
	}
}
