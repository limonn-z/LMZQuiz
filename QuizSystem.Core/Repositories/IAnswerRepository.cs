using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;

namespace QuizSystem.Core.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer> AddAnswerAsync(Answer newAnswer);
        Task EditAnswerAsync(Answer updatedAnswer);
        Task RemoveAnswerByIdAsync(int id);
        Task <Answer?> GetAnswerByIdAsync(int id);
        Task<List<Answer>> GetAllAnswersAsync();
    }
}
