using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace PetMe.Business.Services
{
    public interface IAdoptionRequestService {
        Task<AdoptionRequest> GetAdoptionRequestByIdAsync(int requestId);
        Task<List<AdoptionRequest>> GetAdoptionRequestsByPetIdAsync(int petId);
        Task UpdateAdoptionRequestAsync(AdoptionRequest request);
    }
    public class AdoptionRequestService : IAdoptionRequestService
    {
        private readonly IAdoptionRequestRepository _adoptionRequestRepository;

        public AdoptionRequestService(IAdoptionRequestRepository adoptionRequestRepository)
        {
            _adoptionRequestRepository = adoptionRequestRepository;
        }
        public async Task<AdoptionRequest> GetAdoptionRequestByIdAsync(int requestId)
        {
            var adoptionRequestion = await _adoptionRequestRepository.GetByIdAsync(requestId);
            if (adoptionRequestion == null) { 
                throw new KeyNotFoundException();
                 
            }
            return adoptionRequestion;
        }

        public async Task<List<AdoptionRequest>> GetAdoptionRequestsByPetIdAsync(int petId)
        {
            var adoptionRequestList = await _adoptionRequestRepository.GetAdoptionRequestsByPetIdAsync(petId);
            if (adoptionRequestList == null) {
                throw new KeyNotFoundException();
            }
            return adoptionRequestList;
        }

        public async Task UpdateAdoptionRequestAsync(AdoptionRequest request)
        {
            await _adoptionRequestRepository.UpdateAsync(request);
        }
    }
}
