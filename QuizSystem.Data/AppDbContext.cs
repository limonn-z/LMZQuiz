using Microsoft.EntityFrameworkCore;
using QuizSystem.Core.Models;
using QuizSystem.Core.Models.Junctions;

namespace QuizSystem.Data
{
    /* 
    _ Giải thích: 
        + AppDbContext là một lớp kế thừa từ DbContext, được sử dụng để quản lý kết nối và tương tác với cơ sở dữ liệu.
        + DbContext — kế thừa từ class có sẵn của EF Core, đây là "cầu nối" chính giữa code C# và database thật.
        + DbContextOptions<AppDbContext> options — nơi nhận connection string (sẽ cấu hình ở bước sau).
        + Mỗi DbSet<T> đại diện cho một bảng trong cơ sở dữ liệu, nơi T là kiểu dữ liệu của bảng đó.
     */
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        /* 
        _ Giải thích:
            + OnModelCreating là nơi cấu hình chi tiết thêm cho EF Core, 
              những gì convention (đặt tên đúng chuẩn) không tự suy ra được.
            + Ở đây: ExamQuestion có 2 khóa ngoại (Exam, Question), cả 2 đều dẫn ngược 
              về chung Category qua 2 đường khác nhau → SQL Server không cho phép 
              cả 2 cùng tự động xóa theo (cascade). 
            + Giữ cascade cho đường Exam, đổi đường Question thành Restrict 
              (không tự xóa theo) để phá vỡ xung đột.
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}