using BlazorAuthTemplate.Client.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
	public interface IContactService
	{
		Task<ContactDTO> CreateContactAsync(ContactDTO Contact, string userId);

		Task<ContactDTO> GetContactByIdAsync(int Id, string userId);

		/// <summary>
		/// Updates a contact that belongs to the use
		/// </summary>
		/// <param name="contact"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task UpdateContactAsync(ContactDTO contact, string userId);

		Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId);

		Task<IEnumerable<ContactDTO>> GetContactByCategoryIdAsync(int categoryId, string userId);

		//search looks in FIrstName, LastName and CategoryName
		Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId);
	}
}
