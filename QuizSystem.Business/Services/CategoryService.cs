using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Repositories;
using QuizSystem.Core.Models;

namespace QuizSystem.Business.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> AddCategoryAsync(Category newCategory)
        {
            ValidateCategory(newCategory);
            return await _categoryRepository.AddCategoryAsync(newCategory);
        }

        public async Task EditCategoryAsync(Category updatedCategory)
        {
            ValidateCategory(updatedCategory);
            await _categoryRepository.EditCategoryAsync(updatedCategory);
        }

        public async Task RemoveCategoryByIdAsync(int id)
        {
            await _categoryRepository.RemoveCategoryByIdAsync(id);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            return category ?? throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        private static void ValidateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name cannot be empty.", nameof(category.Name));
        }
    }
}
