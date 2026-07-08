using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;

namespace QuizSystem.Core.Repositories
{
    // Hãy nhớ quy luật:
    //  -> Logic interface phải mô tả đúng chức năng của bảng đó
    public interface IQuestionRepository
    {
        Task<Question> AddQuestionAsync(Question newQuestion);          // Thêm câu hỏi mới 
        Task EditQuestionAsync(Question updatedQuestion);         // Chỉnh sửa câu hỏi 
        Task RemoveQuestionByIdAsync(int id);                           // Xóa câu hỏi bằng id 
        Task<Question> GetQuestionByIdAsync(int id);                    // Lấy câu hỏi theo Id
        Task<List<Question>> GetAllQuestionsAsync();                    // Lấy tất cả câu hỏi
    }
}
