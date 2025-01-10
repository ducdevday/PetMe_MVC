using PetMe.Data.Entities;
using PetMe.Data.Helpers;
using PetMe.DataAccess.Repositories;

namespace PetMe.Business.Services
{
    public interface ILostPetAdService {
        Task CreateLostPetAdAsync(LostPetAd lostPetAd, string city, string district);
        Task<List<LostPetAd>> GetAllLostPetAdsAsync();
        Task<LostPetAd?> GetLostPetAdByIdAsync(int id);
        Task UpdateLostPetAdAsync(LostPetAd lostPetAd);
        Task DeleteLostPetAdAsync(LostPetAd lostPetAd);
    }
    public class LostPetAdService : ILostPetAdService
    {
        private readonly ILostPetAdRepository _lostPetAdRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public LostPetAdService(ILostPetAdRepository lostPetAdRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _lostPetAdRepository = lostPetAdRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }
        public async Task CreateLostPetAdAsync(LostPetAd lostPetAd, string city, string district)
        {
            lostPetAd.LastSeenCity = city;
            lostPetAd.LastSeenDistrict = district;

            await _lostPetAdRepository.CreateLostPetAdAsync(lostPetAd);

            var user = await _userRepository.GetByIdAsync(lostPetAd.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var usersInLocation = await _userRepository.GetUsersByLocationAsync(city, district);

            foreach (var targetUser in usersInLocation)
            {
                var subject = "New Lost Pet Ad Created";
                var body = EmailHelper.GenerateNewLostPetAdEmailBody(lostPetAd, user);
                await _emailService.SendEmailAsync(targetUser.Email, subject, body);
            }
        }

        public async Task<List<LostPetAd>> GetAllLostPetAdsAsync()
        {
            return await _lostPetAdRepository.GetAllAsync();
        }

        public async Task<LostPetAd?> GetLostPetAdByIdAsync(int id)
        {
            var lostPetAd = await _lostPetAdRepository.GetByIdAsync(id);
            if (lostPetAd != null)
            { 
                var user = await _userRepository.GetByIdAsync(lostPetAd.UserId);
                if (user != null)
                    lostPetAd.User = user;
            }
            return lostPetAd;
        }

        public async Task UpdateLostPetAdAsync(LostPetAd lostPetAd)
        {
            await _lostPetAdRepository.UpdateLostPetAdAsync(lostPetAd);
        }

        public async Task DeleteLostPetAdAsync(LostPetAd lostPetAd)
        {
            await _lostPetAdRepository.DeleteLostPetAdAsync(lostPetAd);
        }
    }
}
