using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;

namespace QuizSystem.Core.Repositories
{
    public interface IExamRepository
    {
        Task<Exam> AddExamAsync(Exam newExam);
        Task EditExamAsync(Exam updatedExam);
        Task RemoveExamAsync(Exam exam);
        Task<Exam?> GetExamByIdAsync(int id);
        Task<List<Exam>> GetAllExamsAsync();
    }
}