using QuizSystem.Core.Models;

namespace QuizSystem.Core.Repositories
{
    public interface IExamResultRepository
    {
        Task<ExamResult> AddExamResultAsync(ExamResult newExamResult);
        Task EditExamResultAsync(ExamResult updatedExamResult);
        Task RemoveExamResultAsync(ExamResult examResult);
        Task<ExamResult?> GetExamResultByIdAsync(int id);
        Task<List<ExamResult>> GetAllExamResultsAsync();
    }
}