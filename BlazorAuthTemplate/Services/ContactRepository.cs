using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuthTemplate.Services
{
    public class ContactRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IContactRepository
    {
        public async Task AddCategoriesToContactAsync(int contactId, IEnumerable<int> categoryIds, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact is not null)
            {
                foreach (int categoryId in categoryIds)
                {
                    Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);

                    if (category is not null)
                    {
                        contact.Categories.Add(category);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<Contact> CreateContactAsync(Contact Contact)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Contacts.Add(Contact);
            await context.SaveChangesAsync();
            return Contact;
        }

        public async Task DeleteContactAsync(int Id, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == Id && c.AppUserId == userId);

            if (contact != null)
            {
                context.Contacts.Remove(contact);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Contact>> GetContactByCategoryIdAsync(int categoryId, string userId)
        {
			using ApplicationDbContext context = contextFactory.CreateDbContext();

            List<Contact> contacts = new List<Contact>();

            contacts = await context.Contacts
                                    .Include(c => c.Categories)
                                    .Where(c => c.AppUserId == userId && c.Categories.Any(cat => cat.Id == categoryId))
                                    .ToListAsync();
            return contacts;
		}

        public async Task<Contact?> GetContactByIdAsync(int Id, string userId)
        {
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Contact? contact = await context.Contacts
				.Include(c => c.Categories)
				.FirstOrDefaultAsync(c => c.Id == Id && c.AppUserId == userId);

			return contact;
		}

        public async Task<List<Contact>> GetContactsAsync(string userId)
        {
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			List<Contact> contacts = await context.Contacts
				.Where(c => c.AppUserId == userId)
				.Include(c => c.Categories)
				.ToListAsync();

			return contacts;
		}

        public async Task RemoveCategoriesFromContactAsync(int contactId, string userId)
        {
			using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if(contact is not null)
            {
                contact.Categories.Clear();
                await context.SaveChangesAsync();
            }

		}

        public async Task<List<Contact>> SearchContactsAsync(string searchTerm, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            string normalizedSearchTerm = searchTerm.Trim().ToLower();

            List<Contact> contacts = await context.Contacts
                                                  .Where(c => c.AppUserId == userId) //only include the users contacts
                                                  .Include(c => c.Categories) //get the categories
                                                  .Where(c => string.IsNullOrEmpty(normalizedSearchTerm)
                                                        || c.FirstName!.ToLower().Contains(normalizedSearchTerm)
                                                        || c.LastName!.ToLower().Contains(normalizedSearchTerm)
                                                        || c.Categories.Any(cat => cat.Name!.ToLower().Contains(normalizedSearchTerm))
                                                  ).ToListAsync();
            return contacts;
		}

        public async Task UpdateContactAsync(Contact contact, string userId)
        {
			using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool contactExists = await context.Contacts.AnyAsync(c => c.Id == contact.Id && c.AppUserId == userId);

            if (contactExists)
            {
                ImageUpload? oldImage = null;
                if(contact.Image is not null)
                {
                    //save the new image
                    context.Images.Add(contact.Image);

                    //check for the old image
                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == contact.ImageId);

                    //fix the foreign key or relink to the contact image
                    contact.ImageId = contact.Image.Id;
                }

                context.Contacts.Update(contact);
                await context.SaveChangesAsync();

                if(oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
		}
    }
}
