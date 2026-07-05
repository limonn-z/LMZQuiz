using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Enums;

namespace QuizSystem.Core.Models
{
    // Bảng bao gồm các thuộc tính của "câu hỏi" như: 
    //  + Id: định danh của câu hỏi
    //  + Content: nội dung câu hỏi
    //  + QuestionType (Enum): loại câu hỏi (ví dụ: trắc nghiệm 1 - nhiều đáp án)
    //  + DifficultyLevel (Enum): mức độ khó của câu hỏi (ví dụ: dễ, trung bình, khó)
    //  + CategoryId: danh mục môn học (ví dụ: Toán, Lý, Hóa, ..)
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public int CategoryId { get; set; }
    }
}
