using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class TaskerItem
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Every task must have a name.")]
        public string? Name { get; set; }

        public bool IsComplete { get; set; }

        [Required]
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }

    public static class TaskerItemExtension
    {
        public static TaskerItemDTO ToDTO(this TaskerItem item) => new TaskerItemDTO
        {
            Id = item.Id,
            Name = item.Name,
            IsComplete = item.IsComplete
        };
    }
}
