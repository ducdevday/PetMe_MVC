using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace PetMe.Business.Services
{
    public interface IAdoptionRequestService {
        Task<AdoptionRequest> GetAdoptionRequestByIdAsync(int requestId);
        Task<List<AdoptionRequest>> GetAdoptionRequestsByPetIdAsync(int petId);
        Task<List<AdoptionRequest>> GetPendingRequestsByPetIdAsync(int petId);
        Task<AdoptionRequest?> GetAdoptionRequestByUserAndPetAsync(int userId, int petId);
        Task UpdateAdoptionRequestAsync(AdoptionRequest request);
        Task CreateAdoptionRequestAsync(AdoptionRequest adoptionRequest);

    }
    public class AdoptionRequestService : IAdoptionRequestService
    {
        private readonly IAdoptionRequestRepository _adoptionRequestRepository;
        private readonly IAdoptionRepository _adoptionRepository;


        public AdoptionRequestService(IAdoptionRequestRepository adoptionRequestRepository, IAdoptionRepository adoptionRepository)
        {
            _adoptionRequestRepository = adoptionRequestRepository;
            _adoptionRepository = adoptionRepository;
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

        public async Task<List<AdoptionRequest>> GetPendingRequestsByPetIdAsync(int petId)
        {
            var pendingRequestList = await _adoptionRequestRepository.GetAdoptionRequestsByPetIdAsync(petId);
            if (pendingRequestList == null) {
                throw new KeyNotFoundException();            
            }
            return pendingRequestList;
        }

        public async Task UpdateAdoptionRequestAsync(AdoptionRequest request)
        {
            await _adoptionRequestRepository.UpdateAsync(request);
        }

        public async Task<AdoptionRequest?> GetAdoptionRequestByUserAndPetAsync(int userId, int petId)
        {
            return await _adoptionRequestRepository.GetAdoptionRequestByUserAndPetAsync(userId, petId);
        }

        public async Task CreateAdoptionRequestAsync(AdoptionRequest adoptionRequest)
        {
            if (adoptionRequest == null)
                throw new ArgumentNullException(nameof(adoptionRequest));

            var existingAdoption = await _adoptionRepository.GetAdoptionByPetIdAsync(adoptionRequest.PetId);
            if (existingAdoption != null)
                throw new InvalidOperationException("This pet has already been adopted.");

            await _adoptionRequestRepository.AddAsync(adoptionRequest);
        }
    }
}
