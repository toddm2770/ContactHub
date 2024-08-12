using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Models;

namespace BlazorAuthTemplate.Services
{
    public class TaskerItemService : ITaskerItemService
    {
        private readonly ITaskerItemRepository _repository;

        public TaskerItemService(ITaskerItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskerItemDTO> CreateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            TaskerItem newTaskerItem = new TaskerItem()
            {
                Name = taskerItem.Name,
                IsComplete = taskerItem.IsComplete,
                UserId = userId
            };

            TaskerItem createdTaskerItem = await _repository.CreateTaskerItemAsync(newTaskerItem);

            return createdTaskerItem.ToDTO();
            
        }

        public async Task DeleteTaskerItemAsync(Guid taskerItemId, string userId)
        {
            await _repository.DeleteTaskerItemAsync(taskerItemId, userId);
        }

        public async Task<TaskerItemDTO?> GetTaskerItemByIdAsync(Guid taskerItemId, string userId)
        {
            TaskerItem? taskerItem = await _repository.GetTaskerItemByIdAsync(taskerItemId, userId);
            return taskerItem?.ToDTO();
        }

        public async Task<IEnumerable<TaskerItemDTO>> GetTaskerItemsAsync(string userId)
        {
            IEnumerable<TaskerItem> taskerItems = await _repository.GetTaskerItemsAsync(userId);

            return taskerItems.Select(t => t.ToDTO());
        }

        public async Task UpdateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            TaskerItem? newTaskerItem = await _repository.GetTaskerItemByIdAsync(taskerItem.Id, userId);
            if (newTaskerItem != null) 
            {
                newTaskerItem.Name = taskerItem.Name;
                newTaskerItem.IsComplete = taskerItem.IsComplete;

                await _repository.UpdateTaskerItemAsync(newTaskerItem);
            }
        }
    }
}
