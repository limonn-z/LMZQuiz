using QuizSystem.Business.Services;
using QuizSystem.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace QuizSystem.WPF.ViewModels
{
    public partial class CategoryViewModel(CategoryService categoryService) : ObservableObject
    {
        private readonly CategoryService _categoryService = categoryService;

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();
        [ObservableProperty]
        private string newCategoryName = string.Empty;
        [ObservableProperty]
        private string newCategoryDescription = string.Empty;
        [ObservableProperty]
        private Category? selectedCategory = null;

        [RelayCommand]
        private async Task LoadCategoriesAsync()
        {
            var list = await _categoryService.GetAllCategoriesAsync();
            Categories = new ObservableCollection<Category>(list);
        }

        [RelayCommand]
        private async Task AddCategoryAsync()
        {
            try
            {
                var newCategory = new Category
                {
                    Name = NewCategoryName,
                    Description = NewCategoryDescription
                };

                await _categoryService.AddCategoryAsync(newCategory);

                NewCategoryName = string.Empty;
                NewCategoryDescription = string.Empty;

                await LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        [RelayCommand]
        private async Task RemoveCategoryAsync()
        {
            try
            {
                if (SelectedCategory == null)
                    throw new InvalidOperationException("A category must be selected for removing!");

                await _categoryService.RemoveCategoryAsync(SelectedCategory);
                await LoadCategoriesAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        [RelayCommand]
        private async Task EditCategoryAsync()
        {
            try
            {
                if (SelectedCategory == null)
                    throw new InvalidOperationException("A category must be selected for editing!");

                var updatedCategory = new Category
                {
                    Id = SelectedCategory.Id,
                    Name = NewCategoryName,
                    Description = NewCategoryDescription
                };

                await _categoryService.EditCategoryAsync(updatedCategory);

                NewCategoryName = string.Empty;
                NewCategoryDescription = string.Empty;

                await LoadCategoriesAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error
                );
            }
        }
    }
}
