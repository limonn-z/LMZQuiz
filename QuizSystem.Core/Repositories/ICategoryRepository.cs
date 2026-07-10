using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;

namespace QuizSystem.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category newCategory);         // Thêm môn mới
        Task EditCategoryAsync(Category updatedCategory);               // Chỉnh sửa môn
        Task RemoveCategoryByIdAsync(int id);                           // Xóa môn bằng id
        Task<Category?> GetCategoryByIdAsync(int id);                   // Lấy môn theo Id
        Task<List<Category>> GetAllCategoriesAsync();                   // Lấy tất cả môn
    }
}
