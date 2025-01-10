using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.DataAccess.Repositories
{
    public interface IPetOwnerRepository
    {
        Task AddAsync(PetOwner petOwner);
        Task<PetOwner?> GetPetOwnerByPetIdAsync(int petId);
    }

    public class PetOwnerRepository : IPetOwnerRepository
    {
        private readonly PetMeDbContext _context;

        public PetOwnerRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PetOwner petOwner)
        {
            await _context.PetOwners.AddAsync(petOwner);
            await _context.SaveChangesAsync();
        }

        public async Task<PetOwner?> GetPetOwnerByPetIdAsync(int petId)
        {
            return await _context.PetOwners
                .Include(po => po.User)
                .FirstOrDefaultAsync(po => po.PetId == petId);
        }

    }
}
