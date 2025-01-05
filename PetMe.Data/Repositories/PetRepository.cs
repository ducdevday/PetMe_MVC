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
    public interface IPetRepository {
        Task AddAsync(Pet? pet);
        Task<List<Pet>> GetAllAsync();
        Task<Pet?> GetByIdAsync(int petId);
        Task UpdateAsync(Pet existingPet);
        Task<List<PetOwner>> GetPetOwnersAsync(int petId);
        Task DeleteAsync(Pet pet);
    }
    public class PetRepository : IPetRepository
    {
        private readonly PetMeDbContext _context;

        public PetRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Pet? pet)
        {
            if (pet == null) throw new ArgumentNullException(nameof(pet));
            await _context.Pets.AddAsync(pet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Pet pet)
        {
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Pet>> GetAllAsync()
        {
            return await _context.Pets.ToListAsync();
        }

        public async Task<Pet?> GetByIdAsync(int petId)
        {
            return await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);
        }

        public async Task<List<PetOwner>> GetPetOwnersAsync(int petId)
        {
            return await _context.PetOwners.Where(po => po.PetId == petId).ToListAsync();
        }

        public async Task UpdateAsync(Pet existingPet)
        {
            _context.Pets.Update(existingPet);
            await _context.SaveChangesAsync();
        }
    }
}
