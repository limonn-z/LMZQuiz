using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Core.Models
{
    // Bảng bao gồm các thuộc tính của "đáp án" như:
    //  + Id: định danh của đáp án
    //  + Content: nội dung của đáp án
    //  + IsCorrect: xác định đáp án có đúng hay không
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}
