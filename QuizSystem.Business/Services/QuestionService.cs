using QuizSystem.Core.Repositories;
using QuizSystem.Core.Models;
using QuizSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace QuizSystem.Business.Services
{
    public class QuestionService (IQuestionRepository questionRepository, ICategoryRepository categoryRepository)
    {
        private readonly IQuestionRepository _questionRepository = questionRepository; 
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<Question> AddQuestionAsync(Question newQuestion)
        {
            if (newQuestion == null)
                throw new ArgumentNullException(nameof(newQuestion), "question cannot be null!");
            if (string.IsNullOrWhiteSpace(newQuestion.Content))
                throw new ArgumentException("Question content must not be empty!", nameof(newQuestion));
            if (!Enum.IsDefined(typeof(QuestionType), newQuestion.Type))
                throw new ArgumentException("Question type does not exist!", nameof(newQuestion));
            if (!Enum.IsDefined(typeof(DifficultyLevel), newQuestion.Difficulty))
                throw new ArgumentException("Difficulty level does not exist!", nameof(newQuestion));
            if (await _categoryRepository.GetCategoryByIdAsync(newQuestion.CategoryId) == null)
                throw new ArgumentException("This question must be assigned to the category!", nameof(newQuestion));

            int correctAnswer = newQuestion.Answers.Count(x => x.IsCorrect);
            if (newQuestion.Answers.Count < 2)
                throw new ArgumentException("A question must have at least two answers!", nameof(newQuestion));
            if (newQuestion.Answers.Any(x => string.IsNullOrWhiteSpace(x.Content)))
                throw new ArgumentException("Answer content must not be empty!", nameof(newQuestion));
            if (newQuestion.Type.Equals(QuestionType.SingleChoice) && correctAnswer != 1)
                throw new ArgumentException("A Single-choice question must have exactly one correct answer!", nameof(newQuestion));
            if (newQuestion.Type.Equals(QuestionType.MultipleChoice) && correctAnswer < 1)
                throw new ArgumentException("A multiple-choice question must have at least one correct answer!", nameof(newQuestion));

            return await _questionRepository.AddQuestionAsync(newQuestion);
        }

        public async Task EditQuestionAsync(Question updatedQuestion)
        {
            if (updatedQuestion == null)
                throw new ArgumentNullException(nameof(updatedQuestion), "question cannot be null!");
            if (string.IsNullOrWhiteSpace(updatedQuestion.Content))
                throw new ArgumentException("Question content must not be empty!", nameof(updatedQuestion));
            if (!Enum.IsDefined(typeof(QuestionType), updatedQuestion.Type))
                throw new ArgumentException("Question type does not exist!", nameof(updatedQuestion));
            if (!Enum.IsDefined(typeof(DifficultyLevel), updatedQuestion.Difficulty))
                throw new ArgumentException("Difficulty level does not exist!", nameof(updatedQuestion));
            if (await _categoryRepository.GetCategoryByIdAsync(updatedQuestion.CategoryId) == null)
                throw new ArgumentException("This question must be assigned to the category!", nameof(updatedQuestion));

            await _questionRepository.EditQuestionAsync(updatedQuestion);
        }

        public async Task RemoveQuestionAsync(Question question)
        {
            if (question == null) 
                throw new ArgumentNullException(nameof(question), "Question cannot be null!");
            
            await _questionRepository.RemoveQuestionAsync(question);
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than 0!", nameof(id));

            return await _questionRepository.GetQuestionByIdAsync(id) 
                ?? throw new KeyNotFoundException($"Question with Id {id} not found!");
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllQuestionsAsync();
        }
    }
}
