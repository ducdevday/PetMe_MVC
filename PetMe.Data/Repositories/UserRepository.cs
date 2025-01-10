using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;

namespace PetMe.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersByLocationAsync(string city, string district);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
    }
    public class UserRepository : IUserRepository
    {
        private readonly PetMeDbContext _context;

        public UserRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<User>> GetUsersByLocationAsync(string city, string district)
        {
            return await _context.Users
                .Where(u => u.City == city && u.District == district)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
