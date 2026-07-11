using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Repositories;
using QuizSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizSystem.Data.Repositories
{
    public class ExamResultRepository : IExamResultRepository
    {
        private readonly AppDbContext _context;
        public ExamResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ExamResult> AddExamResultAsync(ExamResult newExamResult)
        {
            await _context.ExamResults.AddAsync(newExamResult);
            await _context.SaveChangesAsync();
            return newExamResult;
        }
        public async Task EditExamResultAsync(ExamResult updaterExamResult)
        {
            var examResultObj = await _context.ExamResults.FindAsync(updaterExamResult.Id);
            if (examResultObj == null)
            {
                throw new Exception($"Exam Result with ID {updaterExamResult.Id} not found for editing!");
            }
            examResultObj.Score = updaterExamResult.Score;
            examResultObj.SubmittedAt = updaterExamResult.SubmittedAt;
            await _context.SaveChangesAsync();
        }
        public async Task RemoveExamResultAsync(ExamResult examResult)
        {
            var examResultObj = await _context.ExamResults.FindAsync(examResult.Id);
            if (examResultObj == null)
            {
                throw new Exception($"Exam Result with ID {examResult.Id} not found for removing!");
            }
            _context.ExamResults.Remove(examResultObj);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ExamResult>> GetAllExamResultsAsync()
        {
            IQueryable<ExamResult> query = _context.ExamResults;
            List<ExamResult> list = await query.ToListAsync();
            return list;
        }
        public async Task<ExamResult?> GetExamResultByIdAsync(int examResultId)
        {
            var examResultObj = await _context.ExamResults.FindAsync(examResultId);
            return examResultObj;
        }
    }
}
