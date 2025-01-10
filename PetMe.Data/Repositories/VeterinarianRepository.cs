using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.DataAccess.Repositories
{
    public interface IVeterinarianRepository
    {
        Task<Veterinarian?> GetByUserIdAsync(int userId);
        Task<Veterinarian> CreateAsync(Veterinarian veterinarian);
        Task UpdateAsync(Veterinarian veterinarian);
        Task<IEnumerable<Veterinarian>> GetAllAsync();
        Task<Veterinarian?> GetByIdAsync(int id);  // Add this method
        Task<List<Veterinarian>> GetAllVeterinariansAsync();
        Task<Veterinarian?> GetApprovedByUserIdAsync(int userId);
    }

    public class VeterinarianRepository : IVeterinarianRepository
    {
        private readonly PetMeDbContext _context;

        public VeterinarianRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task<Veterinarian> CreateAsync(Veterinarian veterinarian)
        {
            _context.Veterinarians.Add(veterinarian);
            await _context.SaveChangesAsync();
            return veterinarian;
        }

        public async Task<IEnumerable<Veterinarian>> GetAllAsync()
        {
            return await _context.Veterinarians
                           .Include(v => v.User) 
                           .ToListAsync();
        }

        public async Task<List<Veterinarian>> GetAllVeterinariansAsync()
        {
            return await _context.Veterinarians
                            .Where(v => v.Status == VeterinarianStatus.Approved)
                            .ToListAsync();
        }

        public async Task<Veterinarian?> GetApprovedByUserIdAsync(int userId)
        {
            return await _context.Veterinarians
                            .FirstOrDefaultAsync(v => v.UserId == userId && v.Status == VeterinarianStatus.Approved);
        }

        public async Task<Veterinarian?> GetByIdAsync(int id)
        {
            return await _context.Veterinarians.FindAsync(id);
        }

        public async Task<Veterinarian?> GetByUserIdAsync(int userId)
        {
            return await _context.Veterinarians.FirstOrDefaultAsync(v => v.UserId == userId);
        }

        public async Task UpdateAsync(Veterinarian veterinarian)
        {
            _context.Veterinarians.Update(veterinarian);
            await _context.SaveChangesAsync();
        }
    }
}
