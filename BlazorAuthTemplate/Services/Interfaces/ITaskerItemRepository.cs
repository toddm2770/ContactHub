using BlazorAuthTemplate.Models;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
    public interface ITaskerItemRepository
    {
        Task<IEnumerable<TaskerItem>> GetTaskerItemsAsync(string userId);

        Task<TaskerItem> CreateTaskerItemAsync(TaskerItem taskerItem);
        Task<TaskerItem?> GetTaskerItemByIdAsync(Guid taskerItem, string userId);
        Task UpdateTaskerItemAsync(TaskerItem taskerItem);    
        Task DeleteTaskerItemAsync(Guid taskerItem, string userId);
    }
}
