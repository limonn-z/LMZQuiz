using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Core.Models
{
    // Bảng bao gồm các thuộc tính của "đáp án" như:
    //  + Id: định danh của đáp án
    //  + Content: nội dung của đáp án
    //  + IsCorrect: xác định đáp án có đúng hay không
    //  + QuestionId: định danh của câu hỏi mà đáp án này thuộc về
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }

        // Một đáp án chỉ thuộc về một câu hỏi
        public Question Question { get; set; } = null!;
    }
}
