using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BlazorAuthTemplate.Client.Models.Enums;
using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace BlazorAuthTemplate.Models
{
    public class Contact
    {
        private DateTimeOffset _birthDate;
        private DateTimeOffset _created;
        public int Id { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters and cannot exceed {1} characters.", MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters and cannot exceed {1} characters.", MinimumLength = 2)]
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName { get { return $"{FirstName} {LastName}"; } }

        [Display(Name = "BirthDay")]
        [DataType(DataType.Date)]
        public DateTimeOffset BirthDate
        {
            get => _birthDate;
            set => _birthDate = value.ToUniversalTime();
        }

        [Required]
        [Display(Name = "Address")]
        public string? Address1 { get; set; }

        private string _address1;

        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        [Required]
        public string? AppUserId { get; set; }
        public virtual ApplicationUser? AppUser { get; set; }

        public Guid? ImageId { get; set; }
        public virtual ImageUpload? Image { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = [];
    }

    public static class ContactItemExtension
    {
        public static ContactDTO ToDTO(this Contact item)
        { 
            ContactDTO contactDTO = new ()
            {

                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                BirthDate = item.BirthDate,
                Address1 = item.Address1,
                Address2 = item.Address2,
                City = item.City,
                State = item.State,
                ZipCode = item.ZipCode,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber,
                Created = item.Created,
                ImageURL = item.ImageId.HasValue ? $"api/uploads/{item.ImageId}" : UploadHelper.DefaultContactImage
            };

            foreach (Category category in item.Categories)
            {
                category.Contacts.Clear();
                contactDTO.Categories.Add(category.ToDTO());
            }           

            return contactDTO;

        }
    }
}
