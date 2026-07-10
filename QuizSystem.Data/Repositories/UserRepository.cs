using QuizSystem.Core.Models;
using QuizSystem.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task EditUserAsync(User updaterUser)
        {
            var userObj = await _context.Users.FindAsync(updaterUser.Id);
            if (userObj == null)
            {
                throw new Exception($"User with ID {updaterUser.Id} not found for editing!");
            }
            userObj.FullName = updaterUser.FullName;
            userObj.Username = updaterUser.Username;
            userObj.PasswordHash = updaterUser.PasswordHash;
            userObj.DateOfBirth = updaterUser.DateOfBirth;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserByIdAsync(int userId)
        {
            var userObj = await _context.Users.FindAsync(userId);
            if (userObj == null)
            {
                throw new Exception($"User with ID {userId} not found for removing!");
            }
            _context.Users.Remove(userObj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            IQueryable<User> query = _context.Users;
            List<User> list = await query.ToListAsync();
            return list;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var userObj = await _context.Users.FindAsync(userId);
            return userObj;
        }
    }
}
