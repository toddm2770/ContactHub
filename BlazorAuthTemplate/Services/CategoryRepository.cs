using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuthTemplate.Services
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ICategoryRepository
    {
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> GetCategoryByIdAsync(int Id, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories
                .Include(c => c.Contacts)
                .FirstOrDefaultAsync(c => c.Id == Id && c.AppUserId == userId);

            return category;
        }
        public async Task<List<Category>> GetCategoriesAsync(string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            List<Category> categories = await context.Categories
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Contacts)
                .ToListAsync();

            return categories;
        }

        public async Task UpdateCategoryAsync(Category category, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            if (await context.Categories.AnyAsync(c => c.Id == category.Id && c.AppUserId == userId)) 
            {
                context.Categories.Update(category);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int id, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.AppUserId == userId);

            if(category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }
    }
}
