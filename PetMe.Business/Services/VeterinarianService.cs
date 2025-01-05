using PetMe.Core.Entities;
using PetMe.Core.Enums;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Business.Services
{
    public interface IVeterinarianService
    {
        Task<Veterinarian> RegisterVeterinarianAsync(int userId, string qualifications, string clinicAddress, string clinicPhoneNumber);
        Task<Veterinarian?> GetByUserIdAsync(int userId);
        Task<Veterinarian?> GetByIdAsync(int id);  // Add this method
        Task ApproveVeterinarianAsync(int veterinarianId);
        Task RejectVeterinarianAsync(int veterinarianId);
        Task<IEnumerable<Veterinarian>> GetAllVeterinariansAsync();

        Task<Veterinarian?> GetApprovedByUserIdAsync(int userId);
    }

    public class VeterinarianService : IVeterinarianService
    {
        private readonly IVeterinarianRepository _veterinarianRepository;

        public VeterinarianService(IVeterinarianRepository veterinarianRepository)
        {
            _veterinarianRepository = veterinarianRepository;
        }

        public async Task ApproveVeterinarianAsync(int veterinarianId)
        {
            var veterinarian = await _veterinarianRepository.GetByIdAsync(veterinarianId);
            if (veterinarian == null)
            {
                throw new KeyNotFoundException("Veterinarian not found");
            }

            veterinarian.Status = VeterinarianStatus.Approved;
            await _veterinarianRepository.UpdateAsync(veterinarian);
        }

        public async Task<IEnumerable<Veterinarian>> GetAllVeterinariansAsync()
        {
            return await _veterinarianRepository.GetAllAsync();
        }

        public async Task<Veterinarian?> GetApprovedByUserIdAsync(int userId)
        {
            return await _veterinarianRepository.GetApprovedByUserIdAsync(userId);
        }

        public async Task<Veterinarian?> GetByIdAsync(int id)
        {
            return await _veterinarianRepository.GetByIdAsync(id);
        }

        public async Task<Veterinarian?> GetByUserIdAsync(int userId)
        {
            return await _veterinarianRepository.GetByUserIdAsync(userId);
        }

        public async Task<Veterinarian> RegisterVeterinarianAsync(int userId, string qualifications, string clinicAddress, string clinicPhoneNumber)
        {
            var veterinarian = new Veterinarian
            {
                UserId = userId,
                Qualifications = qualifications,
                ClinicAddress = clinicAddress,
                ClinicPhoneNumber = clinicPhoneNumber,
                AppliedDate = DateTime.UtcNow,
                Status = VeterinarianStatus.Pending  // Default to Pending status
            };

            return await _veterinarianRepository.CreateAsync(veterinarian);
        }

        public async Task RejectVeterinarianAsync(int veterinarianId)
        {
            var veterinarian = await _veterinarianRepository.GetByIdAsync(veterinarianId);
            if (veterinarian == null)
            {
                throw new KeyNotFoundException("Veterinarian not found");
            }

            veterinarian.Status = VeterinarianStatus.Rejected;
            await _veterinarianRepository.UpdateAsync(veterinarian);
        }
    }
}
