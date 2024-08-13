using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace BlazorAuthTemplate.Client.Services
{
	public class ContactService : IContactService
	{
		private readonly HttpClient _httpClient;

		public ContactService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ContactDTO> CreateContactAsync(ContactDTO contact, string userId)
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/contacts", contact);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<ContactDTO>()
				?? throw new HttpRequestException("Invalid JSON recieved from server");
		}

		public async Task DeleteContactAsync(int contactId, string userId)
		{
			HttpResponseMessage response = await _httpClient.DeleteAsync($"api/contacts/{contactId}");
			response.EnsureSuccessStatusCode();
		}

		public async Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/contacts/{contactId}/email", emailData);
				return response.IsSuccessStatusCode;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task<IEnumerable<ContactDTO>> GetContactByCategoryIdAsync(int categoryId, string userId)
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts?categoryId={categoryId}") ?? [];
		}

		public async Task<ContactDTO?> GetContactByIdAsync(int id, string userId)
		{
			return await _httpClient.GetFromJsonAsync<ContactDTO>($"api/contacts/{id}");
		}

		public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>("api/contacts") ?? [];
		}

		public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
		{
			return await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts/search?query={searchTerm}") ?? [];
		}

		public async Task UpdateContactAsync(ContactDTO contact, string userId)
		{
			HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/contacts/{contact.Id}", contact);
			response.EnsureSuccessStatusCode();
		}
	}
}
