using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;
using QuizSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace QuizSystem.Data.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly AppDbContext _context;
        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Exam> AddExamAsync(Exam newExam)
        {
            await _context.Exams.AddAsync(newExam);
            await _context.SaveChangesAsync();
            return newExam;
        }

        public async Task EditExamAsync(Exam updaterExam)
        {
            var examObj = await _context.Exams.FindAsync(updaterExam.Id);
            if (examObj == null)
            {
                throw new Exception($"Exam with ID {updaterExam.Id} not found for editing!");
            }
            examObj.Title = updaterExam.Title;
            examObj.Description = updaterExam.Description;
            examObj.TimeDuration = updaterExam.TimeDuration;
            examObj.CategoryId = updaterExam.CategoryId;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveExamAsync(Exam exam)
        {
            var examObj = await _context.Exams.FindAsync(exam.Id);
            if (examObj == null)
            {
                throw new Exception($"Exam with ID {exam.Id} not found for removing!");
            }
            _context.Exams.Remove(examObj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            IQueryable<Exam> query = _context.Exams;
            List<Exam> list = await query.ToListAsync();
            return list;
        }

        public async Task<Exam?> GetExamByIdAsync(int examId)
        {
            var examObj = await _context.Exams.FindAsync(examId);
            return examObj;
        }
    }
}
