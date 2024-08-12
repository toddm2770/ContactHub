using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        [Required]
        public string? AppUserId { get; set; }
        public virtual ApplicationUser? AppUser { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = [];



    }
    public static class CategoryItemExtension
    {
        public static CategoryDTO ToDTO(this Category item) 
        {
            CategoryDTO categoryDTO = new()
        {
            Id = item.Id,
            Name = item.Name,
        };

            foreach (Contact contact in item.Contacts) 
            { 
                contact.Categories.Clear();
                categoryDTO.Contacts.Add(contact.ToDTO());
            }

            return categoryDTO;
        }
    }
}
