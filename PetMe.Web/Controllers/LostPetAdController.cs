using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;
using PetMe.Data.Entities;

namespace PetMe.Web.Controllers
{
    public class LostPetAdController : Controller
    {
        private readonly ILostPetAdService _lostPetAdService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IVnAddressService _vpAddressService;

        public LostPetAdController(ILostPetAdService lostPetAdService, IEmailService emailService, IUserService userService, IVnAddressService vnAddressService)
        {
            _lostPetAdService = lostPetAdService;
            _emailService = emailService;
            _userService = userService;
            _vpAddressService = vnAddressService;
        }

        private IActionResult? RedirectToLoginIfNotLoggedIn()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        public async Task<IActionResult> Index()
        {
            var lostPetAds = await _lostPetAdService.GetAllLostPetAdsAsync();
            if (lostPetAds == null)
            {
                TempData["ErrorMessage"] = "Could not retrieve lost pet ads. Please try again later.";
                lostPetAds = new List<LostPetAd>();
            }
            return View(lostPetAds);
        }

        public async Task<IActionResult> Create()
        {
            var redirectResult = RedirectToLoginIfNotLoggedIn();
            if (redirectResult != null) return redirectResult;

            ViewData["Cities"] = await _vpAddressService.GetProvincesAsync();
            ViewData["Districts"] = new List<District>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LostPetAd lostPetAd, string city, string district)
        {
            var redirectResult = RedirectToLoginIfNotLoggedIn();
            if (redirectResult != null) return redirectResult;

            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(district))
            {
                TempData["ErrorMessage"] = "City and District are required.";
                return View(lostPetAd);
            }

            lostPetAd.LastSeenDate = lostPetAd.LastSeenDate.ToUniversalTime();
            var formattedCity = (await _vpAddressService.GetProvincesAsync()).FirstOrDefault(x => x.ProvinceId == city)?.ProvinceName ?? "";
            var formattedDistrict = (await _vpAddressService.GetDisTrictsAsync(int.Parse(city))).FirstOrDefault (x => x.DistrictId == district)?.DistrictName ?? "";
            lostPetAd.CreatedAt = DateTime.UtcNow;
            var username = HttpContext.Session.GetString("Username");
            var user = await _userService.GetUserByUsernameAsync(username);
            lostPetAd.UserId = user.Id;

            await _lostPetAdService.CreateLostPetAdAsync(lostPetAd, formattedCity, formattedDistrict);
            TempData["SuccessMessage"] = "The lost pet ad has been created successfully, and notifications have been sent.";

            return RedirectToAction(nameof(Index));
        }



    }
}
