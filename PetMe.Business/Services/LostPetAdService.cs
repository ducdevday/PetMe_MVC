using PetMe.Core.Entities;
using PetMe.Data.Helpers;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
<<<<<<< HEAD
=======
        private readonly EmailHelper _emailHelper;
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768

        public LostPetAdService(ILostPetAdRepository lostPetAdRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _lostPetAdRepository = lostPetAdRepository;
            _userRepository = userRepository;
            _emailService = emailService;
<<<<<<< HEAD
        }

=======
            _emailHelper = new EmailHelper(); // Initialize EmailHelper
        }

        // Yeni kayıp ilanı oluşturmak için metod
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
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
<<<<<<< HEAD
                var body = EmailHelper.GenerateNewLostPetAdEmailBody(lostPetAd, user);
=======
                var body = _emailHelper.GenerateNewLostPetAdEmailBody(lostPetAd, user);
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
                await _emailService.SendEmailAsync(targetUser.Email, subject, body);
            }
        }

<<<<<<< HEAD
=======
        // Kayıp ilanlarını almak için metod
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        public async Task<List<LostPetAd>> GetAllLostPetAdsAsync()
        {
            return await _lostPetAdRepository.GetAllAsync();
        }

<<<<<<< HEAD
=======
        // Kayıp ilanını ID'ye göre almak için metod
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
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

<<<<<<< HEAD
=======
        // Kayıp ilanını güncellemek için metod
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        public async Task UpdateLostPetAdAsync(LostPetAd lostPetAd)
        {
            await _lostPetAdRepository.UpdateLostPetAdAsync(lostPetAd);
        }

<<<<<<< HEAD
=======
        // Kayıp ilanını silmek için metod
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        public async Task DeleteLostPetAdAsync(LostPetAd lostPetAd)
        {
            await _lostPetAdRepository.DeleteLostPetAdAsync(lostPetAd);
        }
    }
}
