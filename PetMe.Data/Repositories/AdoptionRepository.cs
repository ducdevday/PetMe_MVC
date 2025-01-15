using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;
using PetMe.Data.Enums;

namespace PetMe.DataAccess.Repositories
{
    public interface IAdoptionRepository {
        Task AddAsync(Adoption adoption);
        Task<Adoption?> GetAdoptionByPetIdAsync(int petId);
        Task<bool> IsPetAlreadyAdoptedAsync(int petId);
    }
    public class AdoptionRepository : IAdoptionRepository
    {
        private readonly PetMeDbContext _context;

        public AdoptionRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Adoption adoption)
        {
            await _context.Adoptions.AddAsync(adoption);
            await _context.SaveChangesAsync();
        }

        public async Task<Adoption?> GetAdoptionByPetIdAsync(int petId)
        {
            return await _context.Adoptions
                .FirstOrDefaultAsync(a => a.PetId == petId);
        }

        public async Task<bool> IsPetAlreadyAdoptedAsync(int petId)
        {
            return await _context.Adoptions
                .AnyAsync(a => a.PetId == petId && a.Status == AdoptionStatus.Approved);
        }

        public async Task<bool> HasUserAlreadyRequestedAdoptionAsync(int userId, int petId)
        {
            return await _context.AdoptionRequests
                .AnyAsync(ar => ar.UserId == userId && ar.PetId == petId);
        }
    }
}
