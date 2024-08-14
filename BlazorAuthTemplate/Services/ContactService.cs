using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Helpers;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace BlazorAuthTemplate.Services
{
	public class ContactService : IContactService
	{
		private readonly IContactRepository _repository;
		private readonly IEmailSender _emailSender;

		public ContactService(IContactRepository repository, IEmailSender emailSender)
		{
			_repository = repository;
			_emailSender = emailSender;
		}
		public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId)
		{
			Contact newContact = new Contact()
			{
				FirstName = contactDTO.FirstName,
				LastName = contactDTO.LastName,
				Email = contactDTO.Email,
				PhoneNumber = contactDTO.PhoneNumber,
				Address1 = contactDTO.Address1,
				Address2 = contactDTO.Address2,
				City = contactDTO.City,
				State = contactDTO.State,
				ZipCode = contactDTO.ZipCode,
				BirthDate = contactDTO.BirthDate,
				Created = DateTimeOffset.Now,
				AppUserId = userId

			};

			//add image
			if (contactDTO.ImageURL.StartsWith("data:"))
			{
				newContact.Image = UploadHelper.GetImageUpload(contactDTO.ImageURL);
			}

			//save to the database
			Contact createdContact = await _repository.CreateContactAsync(newContact);

			//add categories
			IEnumerable<int> categoryIds = contactDTO.Categories.Select(c => c.Id);

			//save these categories for the user to the database
			await _repository.AddCategoriesToContactAsync(createdContact.Id, categoryIds, userId);

			//return the contactDTO
			return createdContact.ToDTO();
		}

		public async Task<ContactDTO> GetContactByIdAsync(int Id, string userId)
		{
			Contact? contact = await _repository.GetContactByIdAsync(Id, userId);

			return contact?.ToDTO();
		}

		public async Task UpdateContactAsync(ContactDTO contactDTO, string userId)
		{
			Contact? contact = await _repository.GetContactByIdAsync(contactDTO.Id, userId);

			if (contact is not null)
			{
				contact.FirstName = contactDTO.FirstName;
				contact.LastName = contactDTO.LastName;
				contact.Email = contactDTO.Email;
				contact.PhoneNumber = contactDTO.PhoneNumber;
				contact.Address1 = contactDTO.Address1;
				contact.Address2 = contactDTO.Address2;
				contact.City = contactDTO.City;
				contact.State = contactDTO.State;
				contact.ZipCode = contactDTO.ZipCode;
				contact.BirthDate = contactDTO.BirthDate;
			

			if (contactDTO.ImageURL.StartsWith("data:"))
			{
				contact.Image = UploadHelper.GetImageUpload(contactDTO.ImageURL);
			}
			else
			{
				contact.Image = null;
			}

			contact.Categories.Clear();

			await _repository.UpdateContactAsync(contact, userId);

			//remove the old categories from the database
			await _repository.RemoveCategoriesFromContactAsync(contact.Id, userId);

			//add back the new categories
			IEnumerable<int> selectedCategoryIds = contactDTO.Categories.Select(c => c.Id);
			await _repository.AddCategoriesToContactAsync(contact.Id, selectedCategoryIds, userId);
            }
        }

		public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
		{
			IEnumerable<Contact> contacts = await _repository.GetContactsAsync(userId);
			IEnumerable<ContactDTO> contactDTOs = contacts.Select(c => c.ToDTO());

			return contactDTOs;
		}

		public async Task<IEnumerable<ContactDTO>> GetContactByCategoryIdAsync(int categoryId, string userId)
		{
			IEnumerable<Contact> contacts = await _repository.GetContactByCategoryIdAsync(categoryId, userId);

			return contacts.Select(c => c.ToDTO());
		}

		public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
		{
			IEnumerable<Contact> contacts = await _repository.SearchContactsAsync(searchTerm, userId);

			return contacts.Select(c => c.ToDTO());
		}

		public async Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId)
		{
			try
			{
				Contact? contact = await _repository.GetContactByIdAsync(contactId, userId);
				if (contact == null)
				{
					return false;
				}

				await _emailSender.SendEmailAsync(contact.Email!, emailData.Subject!, emailData.Message!);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}

		public async Task DeleteContactAsync(int contactId, string userId)
		{
			await _repository.DeleteContactAsync(contactId, userId);
		}
	}
}
