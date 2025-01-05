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
    public interface ILostPetAdRepository {
        Task CreateLostPetAdAsync(LostPetAd lostPetAd);
        Task<List<LostPetAd>> GetAllAsync();
        Task<LostPetAd?> GetByIdAsync(int id);
        Task UpdateLostPetAdAsync(LostPetAd lostPetAd);
        Task DeleteLostPetAdAsync(LostPetAd lostPetAd);
    }
    public class LostPetAdRepository : ILostPetAdRepository
    {

        private readonly PetMeDbContext _context;

        public LostPetAdRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task CreateLostPetAdAsync(LostPetAd lostPetAd)
        {
            await _context.LostPetAds.AddAsync(lostPetAd);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LostPetAd>> GetAllAsync()
        {
            return await _context.LostPetAds.ToListAsync();
        }

        public async Task<LostPetAd?> GetByIdAsync(int id)
        {
            return await _context.LostPetAds
                .FirstOrDefaultAsync(ad => ad.Id == id);
        }

        public async Task UpdateLostPetAdAsync(LostPetAd lostPetAd)
        {
            _context.LostPetAds.Update(lostPetAd);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLostPetAdAsync(LostPetAd lostPetAd)
        {
            _context.LostPetAds.Remove(lostPetAd);
            await _context.SaveChangesAsync();
        }
    }
}
