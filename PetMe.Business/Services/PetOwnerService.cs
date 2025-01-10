using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface IPetOwnerService
    {
        Task<PetOwner> GetPetOwnerByPetIdAsync(int petId);
    }

    public class PetOwnerService : IPetOwnerService
    {
        private readonly IPetOwnerRepository _petOwnerRepository;

        public PetOwnerService(IPetOwnerRepository petOwnerRepository)
        {
            _petOwnerRepository = petOwnerRepository;
        }

        public async Task<PetOwner> GetPetOwnerByPetIdAsync(int petId)
        {
            var petOwner = await _petOwnerRepository.GetPetOwnerByPetIdAsync(petId);
            if (petOwner == null)
            {
                throw new KeyNotFoundException("Pet owner not found.");
            }

            return petOwner;
        }
    }
}
