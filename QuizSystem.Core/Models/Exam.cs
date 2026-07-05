using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Core.Models
{
    // Bảng bao gồm các thuộc tính của "đề thi" như:
    // + Id: định danh của đề thi
    // + Title: tiêu đề của đề thi
    // + Description: mô tả của đề thi
    // + TimeDuration: thời lượng làm bài (tính bằng phút)
    // + CategoryId: đề thi này thuộc môn học nào (ví dụ: Toán, Lý, Hóa, ..)
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TimeDuration { get; set; }
        public int CategoryId { get; set; }
    }
}
