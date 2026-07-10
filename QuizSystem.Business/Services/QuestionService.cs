using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuizSystem.Core.Repositories;
using QuizSystem.Core.Models;
using QuizSystem.Core.Enums;

namespace QuizSystem.Business.Services
{
    public class QuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Question> AddQuestionAsync(Question newQuestion)
        {
            ValidateQuestion(newQuestion);
            return await _questionRepository.AddQuestionAsync(newQuestion);
        }

        public async Task EditQuestionAsync(Question updatedQuestion)
        {
            ValidateQuestion(updatedQuestion);
            await _questionRepository.EditQuestionAsync(updatedQuestion);
        }

        public async Task RemoveQuestionByIdAsync(int questionId)
        {
            await _questionRepository.RemoveQuestionByIdAsync(questionId);
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            var question = await _questionRepository.GetQuestionByIdAsync(questionId);
            return question ?? throw new KeyNotFoundException($"Question with ID {questionId} not found.");
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllQuestionsAsync();
        }

        private static void ValidateQuestion(Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question), "Question cannot be null.");
            if (string.IsNullOrWhiteSpace(question.Content))
                throw new ArgumentException("Question content cannot be empty.", nameof(question.Content));
            if (!Enum.IsDefined(typeof(QuestionType), question.Type))
                throw new ArgumentException("Invalid question type.", nameof(question.Type));
            if (!Enum.IsDefined(typeof(DifficultyLevel), question.Difficulty))
                throw new ArgumentException("Invalid difficulty level.", nameof(question.Difficulty));
            if (question.Answers == null || question.Answers.Count < 2)
                throw new ArgumentException("A question must have at least 2 answers.", nameof(question.Answers));
            if (question.Answers.Any(q => string.IsNullOrWhiteSpace(q.Content)))
                throw new ArgumentException("At least one answer has empty content.", nameof(question.Answers));

            int correctAnswers = question.Answers.Count(a => a.IsCorrect);
            if (question.Type == QuestionType.SingleChoice && correctAnswers != 1)
                throw new ArgumentException("Single choice questions must have exactly one correct answer.", nameof(question.Answers));
            if (question.Type == QuestionType.MultipleChoice && correctAnswers < 1)
                throw new ArgumentException("Multiple choice questions must have at least one correct answer.", nameof(question.Answers));
        }
    }
}
 