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

        public async Task<IActionResult> Details(int id)
        {
            var lostPetAd = await _lostPetAdService.GetLostPetAdByIdAsync(id);
            if (lostPetAd == null)
            {
                TempData["ErrorMessage"] = "Lost Pet Ad not found.";
                return RedirectToAction("Index");
            }

            var currentUser = HttpContext.Session.GetString("Username");
            ViewBag.CurrentUser = currentUser;
            return View(lostPetAd);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var redirectResult = RedirectToLoginIfNotLoggedIn();
            if (redirectResult != null) return redirectResult;

            var lostPetAd = await _lostPetAdService.GetLostPetAdByIdAsync(id);
            if (lostPetAd == null)
            {
                TempData["ErrorMessage"] = "Lost Pet Ad not found.";
                return RedirectToAction("Index");
            }

            // Check if the current user is the owner of the ad
            var currentUser = HttpContext.Session.GetString("Username");
            if (lostPetAd.User == null || lostPetAd.User.Username != currentUser)
            {
                TempData["ErrorMessage"] = "You do not have permission to edit this ad.";
                return RedirectToAction("Index");
            }

            // Populate city and district lists
            ViewData["Cities"] = await _vpAddressService.GetProvincesAsync();
            ViewData["Districts"] = new List<District>();

            return View(lostPetAd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LostPetAd updatedLostPetAd, string city, string district)
        {
            var redirectResult = RedirectToLoginIfNotLoggedIn();
            if (redirectResult != null) return redirectResult;

            if (id != updatedLostPetAd.Id)
            {
                TempData["ErrorMessage"] = "Invalid ad ID.";
                return RedirectToAction("Index");
            }

            var lostPetAd = await _lostPetAdService.GetLostPetAdByIdAsync(id);
            if (lostPetAd == null)
            {
                TempData["ErrorMessage"] = "Lost Pet Ad not found.";
                return RedirectToAction("Index");
            }

            // Check if the current user is the owner of the ad
            var currentUser = HttpContext.Session.GetString("Username");
            if (lostPetAd.User == null || lostPetAd.User.Username != currentUser)
            {
                TempData["ErrorMessage"] = "You do not have permission to edit this ad.";
                return RedirectToAction("Index");
            }
            var formattedCity = (await _vpAddressService.GetProvincesAsync()).FirstOrDefault(x => x.ProvinceId == city)?.ProvinceName ?? "";
            var formattedDistrict = (await _vpAddressService.GetDisTrictsAsync(int.Parse(city))).FirstOrDefault(x => x.DistrictId == district)?.DistrictName ?? "";

            // Update the properties of the lost pet ad
            lostPetAd.PetName = updatedLostPetAd.PetName;
            lostPetAd.Description = updatedLostPetAd.Description;
            lostPetAd.LastSeenCity = formattedCity;
            lostPetAd.LastSeenDistrict = formattedDistrict;
            lostPetAd.ImageUrl = updatedLostPetAd.ImageUrl;
            lostPetAd.LastSeenDate = updatedLostPetAd.LastSeenDate.ToUniversalTime();

            try
            {
                await _lostPetAdService.UpdateLostPetAdAsync(lostPetAd);
                TempData["SuccessMessage"] = "The lost pet ad has been updated successfully.";
                return RedirectToAction("Details", new { id = lostPetAd.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while updating the lost pet ad: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var lostPetAd = await _lostPetAdService.GetLostPetAdByIdAsync(id);

            if (lostPetAd == null)
            {
                TempData["ErrorMessage"] = "Lost Pet Ad not found.";
                return RedirectToAction("Index");
            }

            // Check if the current user is the owner of the ad
            var currentUser = HttpContext.Session.GetString("Username");

            if (lostPetAd.User == null)
            {
                TempData["ErrorMessage"] = "The user associated with this ad is not found.";
                return RedirectToAction("Index");
            }

            if (lostPetAd.User.Username != currentUser)
            {
                TempData["ErrorMessage"] = "You do not have permission to delete this ad.";
                return RedirectToAction("Index");
            }

            return View(lostPetAd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lostPetAd = await _lostPetAdService.GetLostPetAdByIdAsync(id);

            if (lostPetAd == null)
            {
                TempData["ErrorMessage"] = "Lost Pet Ad not found.";
                return RedirectToAction("Index");
            }

            // Check if the current user is the owner of the ad
            var currentUser = HttpContext.Session.GetString("Username");

            if (lostPetAd.User == null)
            {
                TempData["ErrorMessage"] = "The user associated with this ad is not found.";
                return RedirectToAction("Index");
            }

            if (lostPetAd.User.Username != currentUser)
            {
                TempData["ErrorMessage"] = "You do not have permission to delete this ad.";
                return RedirectToAction("Index");
            }

            await _lostPetAdService.DeleteLostPetAdAsync(lostPetAd);
            TempData["SuccessMessage"] = "The lost pet ad has been deleted successfully.";
            return RedirectToAction("Index");
        }

    }
}
