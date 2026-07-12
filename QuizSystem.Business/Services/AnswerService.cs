using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;
using QuizSystem.Core.Repositories;

namespace QuizSystem.Business.Services
{
    public class AnswerService(IAnswerRepository answerRepository, IQuestionRepository questionRepository)
    {
        private readonly IAnswerRepository _answerRepository = answerRepository;
        private readonly IQuestionRepository _questionRepository = questionRepository;

        public async Task<Answer> AddAnswerAsync(Answer newAnswer)
        {
            if (newAnswer == null)
                throw new ArgumentNullException(nameof(newAnswer), "newAnswer cannot be null!");
            if (string.IsNullOrWhiteSpace(newAnswer.Content))
                throw new ArgumentException("Answer content must not be empty!", nameof(newAnswer));
            if (await _questionRepository.GetQuestionByIdAsync(newAnswer.QuestionId) == null)
                throw new ArgumentException("This answer must be assigned to an existing question!", nameof(newAnswer));

            return await _answerRepository.AddAnswerAsync(newAnswer);
        }

        public async Task EditAnswerAsync(Answer updatedAnswer)
        {
            if (updatedAnswer == null)
                throw new ArgumentNullException(nameof(updatedAnswer), "updatedAnswer cannot be null!");
            if (string.IsNullOrWhiteSpace(updatedAnswer.Content))
                throw new ArgumentException("Answer content must not be empty!", nameof(updatedAnswer));

            await _answerRepository.EditAnswerAsync(updatedAnswer);
        }

        public async Task RemoveAnswerAsync(Answer answer)
        {
            if (answer == null) 
                throw new ArgumentNullException(nameof(answer), "Answer cannot be null!");

            await _answerRepository.RemoveAnswerAsync(answer);
        }

        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            if (id <= 0) 
                throw new ArgumentException("Id must be greater than 0!", nameof(id));

            return await _answerRepository.GetAnswerByIdAsync(id)
                ?? throw new KeyNotFoundException($"Answer with Id {id} not found!");
        }

        public async Task<List<Answer>> GetAllAnswersAsync()
        {
            return await _answerRepository.GetAllAnswersAsync();
        }
    }
}
