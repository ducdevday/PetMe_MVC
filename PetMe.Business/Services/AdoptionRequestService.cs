using PetMe.Core.Entities;
using PetMe.Data.Repositories;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface IAdoptionRequestService {
        Task<AdoptionRequest> GetAdoptionRequestByIdAsync(int requestId);
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

        public async Task UpdateAdoptionRequestAsync(AdoptionRequest request)
        {
            await _adoptionRequestRepository.UpdateAsync(request);
        }
    }
}
