using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BlazorAuthTemplate.Client.Models.Enums;
using BlazorAuthTemplate.Client.Helpers;

namespace BlazorAuthTemplate.Client.Models
{
    public class ContactDTO
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

        public string? ImageURL { get; set; } = ImageHelper.DefaultContactImage;

        public virtual ICollection<CategoryDTO> Categories { get; set; } = [];
    }
}
