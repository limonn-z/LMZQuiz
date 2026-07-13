using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using QuizSystem.WPF.ViewModels;

namespace QuizSystem.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Xin DI container 1 CategoryViewModel (đã đăng ký ở App.xaml.cs),
            // gắn làm "bộ não" cho cửa sổ này.
            DataContext = App.AppHost.Services.GetRequiredService<CategoryViewModel>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load danh sách ngay khi cửa sổ vừa hiện lên
            var vm = (CategoryViewModel)DataContext;
            await vm.LoadCategoriesCommand.ExecuteAsync(null);  
        }
    }
}