using BlazorAuthTemplate.Client.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
	public interface IContactService
	{
		Task<ContactDTO> CreateContactAsync(ContactDTO Contact, string userId);


		/// <summary>
		/// sends an email to the specified contact based on the user's id
		/// </summary>
		/// <param name="contactId"></param>
		/// <param name="emailData"></param>
		/// <param name="userId"></param>
		/// <returns>true/false if successful or not</returns>
		Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId); 

		/// <summary>
		/// delete contact from the database based on the user's id
		/// </summary>
		/// <param name="contactId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task DeleteContactAsync(int contactId, string userId);

		Task<ContactDTO?> GetContactByIdAsync(int Id, string userId);

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
