using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using QuizSystem.Core.Repositories;

namespace QuizSystem.Data.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;
        public AnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Answer> AddAnswerAsync(Answer newAnswer)
        {
            await _context.Answers.AddAsync(newAnswer);
            await _context.SaveChangesAsync();
            return newAnswer;
        }

        public async Task EditAnswerAsync(Answer updaterAnswer)
        {
            var answerObj = await _context.Answers.FindAsync(updaterAnswer.Id);
            if (answerObj == null)
            {
                throw new Exception($"Answer with ID {updaterAnswer.Id} not found for editing!");
            }
            answerObj.Content = updaterAnswer.Content;
            answerObj.IsCorrect = updaterAnswer.IsCorrect;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAnswerByIdAsync(int answerId)
        {
            var answerObj = await _context.Answers.FindAsync(answerId);
            if (answerObj == null)
            {
                throw new Exception($"Answer with ID {answerId} not found for removing!");
            }
            _context.Answers.Remove(answerObj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Answer>> GetAllAnswersAsync()
        {
            IQueryable<Answer> query = _context.Answers;
            List<Answer> list = await query.ToListAsync();
            return list;
        }

        public async Task<Answer> GetAnswerByIdAsync(int answerId)
        {
            var answerObj = await _context.Answers.FindAsync(answerId);
            if (answerObj == null)
            {
                throw new Exception($"Answer with ID {answerId} not found!");
            }
            return answerObj;
        }
    }
}
