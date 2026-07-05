using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Core.Models.Junctions
{
    // Bảng trung gian giữa Exam và Question
    // Ý nghĩa: Mỗi Exam có thể có nhiều Question và mỗi Question có thể thuộc nhiều Exam
    /* 
        Suy ra, mỗi đề thi sẽ có nhiều dòng câu hỏi tương ứng, mà mỗi dòng câu hỏi chỉ thuộc về đúng 1 đề thi 
        <=>  1 câu hỏi cũng có thể xuất hiện ở nhiều dòng khác nhau — vì câu đó có thể được xuất hiện ở nhiều đề khác nhau.
    */
    public class ExamQuestion
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
    }
}
