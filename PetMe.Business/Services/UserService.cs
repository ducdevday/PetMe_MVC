using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface IUserService {
        Task<User> AuthenticateAsync(string username, string password);
        Task RegisterAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task<List<User>> GetUsersByLocationAsync(string city, string district);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetAllAsync()
                                .ContinueWith(task => task.Result.FirstOrDefault(u => u.Username == username));

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                user.LastLoginDate = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
                return user;
            }
            throw new KeyNotFoundException("User not found.");
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return user;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }

            return await _userRepository.GetAllAsync()
                .ContinueWith(task => task.Result.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<List<User>> GetUsersByLocationAsync(string city, string district)
        {
            var usersInLocation = (await _userRepository.GetAllAsync()) 
             .Where(u => u.City == city && u.District == district) 
             .ToList(); 

            return usersInLocation;
        }

        public async Task RegisterAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new ArgumentException("Password cannot be empty.");
            }

            var existingUser = await _userRepository.GetAllAsync()
                                                     .ContinueWith(task => task.Result.FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email));

            if (existingUser != null)
            {
                throw new ArgumentException("Username or email already exists.");
            }

            if (user.Roles == null || !user.Roles.Any())
            {
                user.Roles = new List<string> { "User" };
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;

            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            bool isUpdated = false;

            if (!string.IsNullOrEmpty(user.Username) && user.Username != existingUser.Username)
            {
                existingUser.Username = user.Username;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
            {
                existingUser.Email = user.Email;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber) && user.PhoneNumber != existingUser.PhoneNumber)
            {
                existingUser.PhoneNumber = user.PhoneNumber;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(user.Address) && user.Address != existingUser.Address)
            {
                existingUser.Address = user.Address;
                isUpdated = true;
            }

            if (user.DateOfBirth != default && user.DateOfBirth != existingUser.DateOfBirth)
            {
                existingUser.DateOfBirth = user.DateOfBirth;
                isUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(user.PasswordHash) && user.PasswordHash != existingUser.PasswordHash)
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                isUpdated = true;
            }

            if (user.LastLoginDate.HasValue && user.LastLoginDate != existingUser.LastLoginDate)
            {
                existingUser.LastLoginDate = user.LastLoginDate.Value;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(user.ProfileImageUrl) && user.ProfileImageUrl != existingUser.ProfileImageUrl)
            {
                existingUser.ProfileImageUrl = user.ProfileImageUrl;
                isUpdated = true;
            }

            if (isUpdated)
            {
                await _userRepository.UpdateAsync(existingUser);
            }
        }
    }
}
