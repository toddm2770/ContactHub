using BlazorAuthTemplate.Client.Models;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
    public interface ITaskerItemService
    {
        Task<TaskerItemDTO> CreateTaskerItemAsync(TaskerItemDTO taskerItem, string userId);
        Task<IEnumerable<TaskerItemDTO>> GetTaskerItemsAsync(string userId);
        Task<TaskerItemDTO?> GetTaskerItemByIdAsync(Guid taskerItemId, string userId);
        Task UpdateTaskerItemAsync(TaskerItemDTO taskerItem, string userId);
        Task DeleteTaskerItemAsync(Guid taskerItemId, string userId);
    }
}
