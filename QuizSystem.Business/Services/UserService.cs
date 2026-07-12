using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;
using QuizSystem.Core.Repositories;

namespace QuizSystem.Business.Services
{
    public class UserService(IUserRepository userRepository)
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User> AddUserAsync(User newUser)
        {
            ValidateUser(newUser);
            return await _userRepository.AddUserAsync(newUser);
        }

        public async Task EditUserAsync(User updatedUser)
        {
            ValidateUser(updatedUser);
            await _userRepository.EditUserAsync(updatedUser);
        }

        public async Task RemoveUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null!");

            await _userRepository.RemoveUserAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than 0!", nameof(id));

            return await _userRepository.GetUserByIdAsync(id)
                ?? throw new KeyNotFoundException($"User with Id {id} not found!");
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        private static void ValidateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null!");
            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new ArgumentException("Full name must not be empty!", nameof(user));
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username must not be empty!", nameof(user));
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("Password must not be empty!", nameof(user));

            const int minAge = 12;
            const int maxAge = 100;
            int currentAge = DateTime.Today.Year - user.DateOfBirth.Year;
            if (user.DateOfBirth.Date > DateTime.Today.AddYears(-currentAge))
                currentAge--;

            if (currentAge < minAge || currentAge > maxAge)
                throw new ArgumentException($"Age must be between {minAge} and {maxAge}!", nameof(user));
        }
    }
}