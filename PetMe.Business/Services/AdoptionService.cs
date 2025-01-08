using PetMe.Core.Entities;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface IAdoptionService {
        Task<Adoption?> GetAdoptionByPetIdAsync(int petId);
        Task CreateAdoptionRequestAsync(AdoptionRequest adoptionRequest);
        Task<AdoptionRequest?> GetAdoptionRequestByUserAndPetAsync(int userId, int petId);
        Task CreateAdoptionAsync(Adoption adoption);
    }

    public class AdoptionService : IAdoptionService
    {
        private readonly IAdoptionRepository _adoptionRepository;

        public AdoptionService(IAdoptionRepository adoptionRepository)
        {
            _adoptionRepository = adoptionRepository ?? throw new ArgumentNullException(nameof(adoptionRepository));
        }

        public async Task CreateAdoptionAsync(Adoption adoption)
        {
            if (adoption == null)
                throw new ArgumentNullException(nameof(adoption));

            var isAlreadyAdopted = await _adoptionRepository.IsPetAlreadyAdoptedAsync(adoption.PetId);
            if (isAlreadyAdopted)
                throw new InvalidOperationException("This pet has already been adopted.");

            await _adoptionRepository.AddAsync(adoption);
        }

        public async Task<Adoption?> GetAdoptionByPetIdAsync(int petId)
        {
            return await _adoptionRepository.GetAdoptionByPetIdAsync(petId);
        }

        public async Task CreateAdoptionRequestAsync(AdoptionRequest adoptionRequest)
        {
            if (adoptionRequest == null)
                throw new ArgumentNullException(nameof(adoptionRequest));

            var existingAdoption = await _adoptionRepository.GetAdoptionByPetIdAsync(adoptionRequest.PetId);
            if (existingAdoption != null)
                throw new InvalidOperationException("This pet has already been adopted.");

            await _adoptionRepository.AddAsync(adoptionRequest);
        }

        public async Task<AdoptionRequest?> GetAdoptionRequestByUserAndPetAsync(int userId, int petId)
        {
            return await _adoptionRepository.GetAdoptionRequestByUserAndPetAsync(userId, petId);
        }
    }
}
