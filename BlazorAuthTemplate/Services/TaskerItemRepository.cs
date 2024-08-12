using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuthTemplate.Services
{
    public class TaskerItemRepository : ITaskerItemRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public TaskerItemRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<TaskerItem> CreateTaskerItemAsync(TaskerItem taskerItem)
        {
            using var context = _contextFactory.CreateDbContext();

            await context.TaskerItems.AddAsync(taskerItem);
            await context.SaveChangesAsync();

            return taskerItem;
        }

        public async Task DeleteTaskerItemAsync(Guid taskerItemId, string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            TaskerItem? taskerItem = await context.TaskerItems.FirstOrDefaultAsync(t => t.Id == taskerItemId && t.UserId == userId);

            if (taskerItem != null)
            {
                context.TaskerItems.Remove(taskerItem);
                await context.SaveChangesAsync();
            }
        }
        public async Task<TaskerItem?> GetTaskerItemByIdAsync(Guid taskerItemId, string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            TaskerItem? taskerItem = await context.TaskerItems.FirstOrDefaultAsync(t => t.Id == taskerItemId && t.UserId == userId);

            return taskerItem;
        }

        public async Task<IEnumerable<TaskerItem>> GetTaskerItemsAsync(string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            IEnumerable<TaskerItem> taskerItems = await context.TaskerItems.Where(t => t.UserId == userId).ToListAsync();

            return taskerItems;

        }

        public async Task UpdateTaskerItemAsync(TaskerItem taskerItem)
        {
            using var context = _contextFactory.CreateDbContext();

            if(await context.TaskerItems.AnyAsync(t => t.Id == taskerItem.Id && t.UserId == taskerItem.UserId))
            {
                context.Update(taskerItem);
                await context.SaveChangesAsync();
            }
        }
    }
}
