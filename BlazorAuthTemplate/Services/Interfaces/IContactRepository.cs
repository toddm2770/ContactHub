using BlazorAuthTemplate.Models;

namespace BlazorAuthTemplate.Services.Interfaces
{
    public interface IContactRepository
    {
        //create
        Task<Contact> CreateContactAsync(Contact Contact);
        
        //read
        Task<List<Contact>> GetContactsAsync(string userId);
        Task<Contact?> GetContactByIdAsync(int Id, string userId);
        Task<IEnumerable<Contact>> GetContactByCategoryIdAsync(int categoryId, string userId);
        Task<List<Contact>> SearchContactsAsync(string searchTerm, string userId);

        //update
        Task UpdateContactAsync(Contact contact, string userId);
        Task RemoveCategoriesFromContactAsync(int contactId, string userId);
        Task AddCategoriesToContactAsync(int contactId, IEnumerable<int> categoryIds, string userId);

        //delete
        Task DeleteContactAsync(int Id, string userId);
    }
}
