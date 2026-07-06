using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Core.Models
{
    // Môn học bao gồm các thuộc tính như:
    //  + Id: định danh của môn học
    //  + Name: tên môn học
    //  + Description: mô tả về môn học
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation property
        public List<Question> Questions { get; set; } = new List<Question>(); // Một môn học có thể có nhiều câu hỏi
        public List<Exam> Exams { get; set; } = new List<Exam>(); // Một môn học có thể có nhiều đề thi
    }
}
