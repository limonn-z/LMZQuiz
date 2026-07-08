using System;
using System.Collections.Generic;
using System.Text;
using QuizSystem.Core.Models;

namespace QuizSystem.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User newUser);
        Task EditUserAsync(User updatedUser);
        Task RemoveUserByIdAsync(int id);
        Task<User> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
    }
}
