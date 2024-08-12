using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Client.Models
{
	public class EmailData
	{
		[Required]
		public string? Recipients { get; set; }
		[Required]
		[Length(5,160, ErrorMessage = "The {0} must be between {1} and {2}.")]
		public string? Subject { get; set; }
		[Required]
		[MinLength(10, ErrorMessage = "The {0} must be at least {1} characters long.")]
		public string? Message { get; set; }
	}
}
