using Microsoft.EntityFrameworkCore;
using QuizSystem.Core.Models;  
using QuizSystem.Core.Repositories;

namespace QuizSystem.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        // Biến tạo 1 lần duy nhất, cất vào _context, rồi các hàm khác dùng lại
        private readonly AppDbContext _context;

        // Hàm khởi tạo của CategoryRepository, nhận vào một đối tượng AppDbContext để tương tác với cơ sở dữ liệu.
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
            if (categoryObj == null) {
                throw new Exception($"{updaterCategory.Name} category with ID {updaterCategory.Id} not found to edit!");
            }
            categoryObj.Name = updaterCategory.Name;
            categoryObj.Description = updaterCategory.Description;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCategoryByIdAsync(int categoryId)
        {
            var categoryObj = await _context.Categories.FindAsync(categoryId);
            if (categoryObj == null)
            {
                throw new Exception($"Category with ID {categoryId} not found to remove!");
            }
            _context.Categories.Remove(categoryObj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var list = new List<Category>();
            list = await _context.Categories.ToListAsync();
            return list;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            var categoryObj = await _context.Categories.FindAsync(categoryId);
            if(categoryObj == null) throw new Exception($"Category with ID {categoryId} not found!");
            return categoryObj;
        }
    }
}
