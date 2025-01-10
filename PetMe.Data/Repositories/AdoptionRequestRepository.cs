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

    public interface IAdoptionRequestRepository {
        Task<List<AdoptionRequest>> GetAdoptionRequestsByPetIdAsync(int petId);
        Task<List<AdoptionRequest>> GetPendingRequestsByPetIdAsync(int petId);
        Task<AdoptionRequest?> GetByIdAsync(int adoptionRequestId);
        Task UpdateAsync(AdoptionRequest adoptionRequest);
    }
    public class AdoptionRequestRepository : IAdoptionRequestRepository
    {
        private readonly PetMeDbContext _context;

        public AdoptionRequestRepository(PetMeDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdoptionRequest>> GetAdoptionRequestsByPetIdAsync(int petId)
        {
            return await _context.AdoptionRequests
                                .Include(ar => ar.User)
                                .Include(ar => ar.Pet)
                                .Where(ar => ar.PetId == petId)
                                .ToListAsync();
        }

        public async Task<List<AdoptionRequest>> GetPendingRequestsByPetIdAsync(int petId)
        {
            return await _context.AdoptionRequests
                .Where(r => r.PetId == petId && r.Status == AdoptionStatus.Pending)
                .ToListAsync();
        }

        public async Task<AdoptionRequest?> GetByIdAsync(int adoptionRequestId)
        {
            return await _context.AdoptionRequests
                .Include(r => r.Pet) 
                .Include(r => r.User) 
                .FirstOrDefaultAsync(r => r.Id == adoptionRequestId);
        }

        public async Task UpdateAsync(AdoptionRequest adoptionRequest)
        {
            _context.AdoptionRequests.Update(adoptionRequest);
            await _context.SaveChangesAsync();
        }
    }
}
