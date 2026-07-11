using Microsoft.EntityFrameworkCore;
using QuizSystem.Core.Models;
using QuizSystem.Core.Repositories;

namespace QuizSystem.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        // Biến tạo 1 lần duy nhất, cất vào _context, rồi các hàm khác dùng lại
        private readonly AppDbContext _context;

        // Nhận vào một đối tượng AppDbContext để tương tác với cơ sở dữ liệu.
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        // Hoàn thiện phần định nghĩa của các phương thức trong ICategoryRepository
        public async Task<Category> AddCategoryAsync(Category newCategory)
        {
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return newCategory;
        }

        public async Task EditCategoryAsync(Category updaterCategory)
        {
            var categoryObj = await _context.Categories.FindAsync(updaterCategory.Id);
            if (categoryObj == null)
            {
                throw new Exception($"Category with ID {updaterCategory.Id} not found for editing!");
            }
            categoryObj.Name = updaterCategory.Name;
            categoryObj.Description = updaterCategory.Description;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            var categoryObj = await _context.Categories.FindAsync(category.Id);
            if (categoryObj == null)
            {
                throw new Exception($"Category with ID {category.Id} not found for removing!");
            }
            _context.Categories.Remove(categoryObj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            IQueryable<Category> query = _context.Categories;
            List<Category> list = await query.ToListAsync();
            return list;
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            var categoryObj = await _context.Categories.FindAsync(categoryId);
            return categoryObj;
        }
    }
}
