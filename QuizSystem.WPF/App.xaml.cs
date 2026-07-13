using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizSystem.Data;
using System.Windows;
using QuizSystem.Business.Services;
using QuizSystem.Core.Repositories;
using QuizSystem.Data.Repositories;
using QuizSystem.WPF.ViewModels;

namespace QuizSystem.WPF
{
    public partial class App : Application
    {
        /* 
        _ Giải thích:
            + AppHost là "hộp chứa DI" (Dependency Injection) của toàn bộ app.
            + Bất kỳ đâu trong app cần AppDbContext, sẽ xin qua AppHost này,
              không cần tự tay "new AppDbContext(...)" mỗi lần.
            + static để chỉ có DUY NHẤT 1 hộp chứa dùng chung cho cả app.
         */
        public static IHost AppHost { get; private set; } = null!;

        /* 
        _ Giải thích:
            + OnStartup là hàm chạy ĐẦU TIÊN, ngay khi app WPF khởi động,
              trước cả khi MainWindow hiện lên.
            + Đây là nơi "setup" mọi thứ cần thiết trước khi app thật sự chạy.
         */
        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost = Host.CreateDefaultBuilder()

                // Đọc file appsettings.json lên, lấy connection string từ đó
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })

                // Đăng ký AppDbContext vào "hộp chứa DI"
                .ConfigureServices((context, services) =>
                {
                    // Lấy đúng chuỗi kết nối, khớp tên "DefaultConnection" đã đặt trong JSON
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                    // Từ giờ, mỗi khi ai cần AppDbContext, tự động tạo với connection string này
                    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
                    services.AddScoped<ICategoryRepository, CategoryRepository>();
                    services.AddScoped<IQuestionRepository, QuestionRepository>();
                    services.AddScoped<IAnswerRepository, AnswerRepository>();
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<CategoryService>();
                    services.AddScoped<QuestionService>();
                    services.AddScoped<AnswerService>();
                    services.AddScoped<UserService>();
                    services.AddScoped<CategoryViewModel>();
                })

                .Build();

            base.OnStartup(e);
        }
    }
}