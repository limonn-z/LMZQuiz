using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Core.Models
{
    // Lớp User đại diện cho một người dùng trong hệ thống bao gồm các thuộc tính:
    //  + Id: Mã định danh duy nhất của người dùng.
    //  + FullName: Tên đầy đủ của người dùng.
    //  + Username: Tên đăng nhập của người dùng.
    //  + Password: Mật khẩu của người dùng (đã được băm).
    //  + DateOfBirth: Ngày sinh của người dùng.
    //  + CreateAt: Ngày tạo tài khoản của người dùng.
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // mật khẩu đã được băm
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public List<ExamResult> ExamResults { get; set; } = new List<ExamResult>(); // Một người dùng có thể có nhiều kết quả thi
    }
}
