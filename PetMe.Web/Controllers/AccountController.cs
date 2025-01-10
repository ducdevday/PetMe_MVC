using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;
using PetMe.Core.Entities;
using PetMe.Data.Entities;

namespace PetMe.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IVnAddressService _vpnAddressService;

        public AccountController(IUserService userService, IVnAddressService vpnAddressService)
        {
            _userService = userService;
            _vpnAddressService = vpnAddressService;
        }

        public async Task<IActionResult> Register()
        {
           
            ViewData["Cities"] = await _vpnAddressService.GetProvincesAsync();
            ViewData["Districts"] = new List<District>();  // Initial empty list for districts
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string phoneNumber, string address, DateTime dateOfBirth, string profileImageUrl, string city, string district)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = password,
                PhoneNumber = phoneNumber,
                Address = address,
                DateOfBirth = dateOfBirth.ToUniversalTime(),
                ProfileImageUrl = profileImageUrl,
                City = city,
                District = district
            };

            await _userService.RegisterAsync(user);
            return RedirectToAction("Login");
        }


        [HttpGet]
        public async Task<IActionResult> GetDistricts([FromQuery] int provinceId)
        {
            var districts = await _vpnAddressService.GetDisTrictsAsync(provinceId);
            return Json(districts); 
        }

        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password) {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            try
            {
                var user = await _userService.AuthenticateAsync(username, password);
                if (user != null) { 
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }


}
