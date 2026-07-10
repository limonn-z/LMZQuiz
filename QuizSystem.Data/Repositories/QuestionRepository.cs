using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Repositories;
using QuizSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizSystem.Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Question> AddQuestionAsync(Question newQuestion)
        {
            await _context.Questions.AddAsync(newQuestion);
            await _context.SaveChangesAsync();
            return newQuestion;
        }
        
        public async Task EditQuestionAsync(Question updaterQuestion)
        {
            var questionObj = await _context.Questions.FindAsync(updaterQuestion.Id);
            if (questionObj == null)
            {
                throw new Exception($"Question with ID {updaterQuestion.Id} not found for editing!");
            }
            questionObj.Content = updaterQuestion.Content;
            questionObj.Type = updaterQuestion.Type;
            questionObj.Difficulty = updaterQuestion.Difficulty;
            questionObj.CategoryId = updaterQuestion.CategoryId;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveQuestionByIdAsync(int questionId)
        {
            var questionObj = await _context.Questions.FindAsync(questionId);
            if (questionObj == null)
            {
                throw new Exception($"Question with ID {questionId} not found for removing!");
            }
            _context.Questions.Remove(questionObj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            IQueryable<Question> query = _context.Questions;
            List<Question> list = await query.ToListAsync();
            return list;
        }

        public async Task<Question?> GetQuestionByIdAsync(int questionId)
        {
            var questionObj = await _context.Questions.FindAsync(questionId);
            return questionObj;
        }
    }
}
