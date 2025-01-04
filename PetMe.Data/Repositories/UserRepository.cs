using Microsoft.EntityFrameworkCore;
using PetMe.Core.Entities;
using PetMe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.DataAccess.Repositories
{
    public interface IUserRepository {
        Task<List<User>> GetUsersByLocationAsync(string city, string district);
    }
    public class UserRepository : IUserRepository, IRepository<User>
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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
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
    }
}
