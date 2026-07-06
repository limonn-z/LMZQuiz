using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Core.Models
{
    // Lớp ExamResult đại diện cho kết quả thi của một người dùng trong hệ thống bao gồm các thuộc tính:
    // + Id: Mã định danh duy nhất của kết quả thi.
    // + Score: Điểm số của người dùng trong kỳ thi.
    // + UserId: Mã định danh của người dùng tham gia kỳ thi.
    // + ExamId: Mã định danh của kỳ thi mà người dùng tham gia.
    // + StartedAt: thời điểm user bắt đầu làm đề thi.
    // + SubmittedAt: thời điểm user nộp bài làm.

    public class ExamResult
    {
        public int Id { get; set; }
        public double Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }

        // Navigation properties
        public Exam Exam { get; set; } = null!; // Một kết quả thi thuộc về một đề thi
        public User User { get; set; } = null!; // Một kết quả thi thuộc về một người dùng
    }
}
