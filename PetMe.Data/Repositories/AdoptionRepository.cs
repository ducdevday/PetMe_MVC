using Microsoft.EntityFrameworkCore;
using PetMe.Core.Entities;
using PetMe.Core.Enums;
using PetMe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.DataAccess.Repositories
{
    public interface IAdoptionRepository {
        Task AddAsync(Adoption adoption);
        Task AddAsync(AdoptionRequest adoptionRequest);
        Task<Adoption?> GetAdoptionByPetIdAsync(int petId);
        Task<bool> IsPetAlreadyAdoptedAsync(int petId);
        Task<AdoptionRequest?> GetAdoptionRequestByUserAndPetAsync(int userId, int petId);
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

        public async Task AddAsync(AdoptionRequest adoptionRequest)
        {
            await _context.AdoptionRequests.AddAsync(adoptionRequest);
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

        public async Task<AdoptionRequest?> GetAdoptionRequestByUserAndPetAsync(int userId, int petId)
        {
            return await _context.AdoptionRequests
                .FirstOrDefaultAsync(ar => ar.UserId == userId && ar.PetId == petId);
        }
    }
}
