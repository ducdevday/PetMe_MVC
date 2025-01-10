using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;

namespace PetMe.Web.Controllers
{
    public class PetController : Controller
    {
        private readonly IPetService _petService;
        private readonly IUserService _userService;
        private readonly IAdoptionService _adoptionService;
        private readonly IEmailService _emailService;

        public PetController(
            IPetService petService,
            IUserService userService,
            IAdoptionService adoptionService,
            IEmailService emailService)
        {
            _petService = petService;
            _userService = userService;
            _adoptionService = adoptionService;
            _emailService = emailService;
        }

        private async Task<User?> GetLoggedInUserAsync() {
            var username = HttpContext.Session.GetString("Username");
            if (username == null) return null;
            return await _userService.GetUserByUsernameAsync(username);
        }

        private IActionResult? RedirectToLoginIfNotLoggedIn() {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        public IActionResult Create()
        {
            var loginRedirect = RedirectToLoginIfNotLoggedIn();
            if (loginRedirect != null) return loginRedirect;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pet pet)
        {
            var loginRedirect = RedirectToLoginIfNotLoggedIn();
            if (loginRedirect != null) return loginRedirect;

            var user = await GetLoggedInUserAsync();

            if (ModelState.IsValid)
            {
                await _petService.CreatePetAsync(pet);

                var petOwner = new PetOwner
                {
                    PetId = pet.Id,
                    UserId = user.Id,
                    OwnershipDate = DateTime.Now
                };

                await _petService.AssignPetOwnerAsync(petOwner);

                return RedirectToAction("Index", "Adoption");
            }

            return View(pet);
        }


    }
}
