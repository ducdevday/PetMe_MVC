using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.Data.Helpers;
using PetMe.DataAccess.Repositories;

namespace PetMe.Web.Controllers
{
    public class PetController : Controller
    {
        private readonly IPetService _petService;
        private readonly IUserService _userService;
        private readonly IAdoptionService _adoptionService;
        private readonly IAdoptionRequestService _adoptionRequestService;
        private readonly IEmailService _emailService;

        public PetController(
            IPetService petService,
            IUserService userService,
            IAdoptionService adoptionService,
            IAdoptionRequestService adoptionRequestService,
            IEmailService emailService)
        {
            _petService = petService;
            _userService = userService;
            _adoptionService = adoptionService;
            _adoptionRequestService = adoptionRequestService;
            _emailService = emailService;
        }

        private async Task<User?> GetLoggedInUserAsync()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null) return null;
            return await _userService.GetUserByUsernameAsync(username);
        }

        private IActionResult? RedirectToLoginIfNotLoggedIn()
        {
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
            ViewData["Species"] = new List<Species> { Species.Dog, Species.Cat, Species.Hamster, Species.Rabbit };
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
                    OwnershipDate = DateTime.UtcNow
                };

                await _petService.AssignPetOwnerAsync(petOwner);

                return RedirectToAction("Index", "Adoption");
            }

            return View(pet);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                ViewBag.ErrorMessage = "Pet Not Found";
            }

            var adoptionRequests = await _adoptionService.GetAdoptionByPetIdAsync(id);
            var adoption = await _adoptionService.GetAdoptionByPetIdAsync(id);

            var user = await GetLoggedInUserAsync();
            var isUserLoggedIn = user != null;
            var isOwner = user != null && await _petService.IsUserOwnerOfPetAsync(id, user.Id);
            var hasAdoptionRequest = user != null && await _adoptionService.GetAdoptionRequestByUserAndPetAsync(user.Id, id) != null;

            ViewBag.AdoptionStatus = adoption != null
            ? "This pet has already been adopted."
            : "This pet is available for adoption.";

            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            ViewBag.Adoption = adoption;
            ViewBag.IsOwner = isOwner;
            ViewBag.AdoptionRequests = adoptionRequests;
            ViewBag.HasAdoptionRequest = hasAdoptionRequest;

            return View(pet);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var loginRedirect = RedirectToLoginIfNotLoggedIn();
            if (loginRedirect != null) return loginRedirect;

            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                ViewBag.ErrorMessage = "Pet not found.";
                return View("Error");
            }

            var adoption = await _adoptionService.GetAdoptionByPetIdAsync(id);
            if (adoption != null)
            {
                ViewBag.ErrorMessage = "This pet has already been adopted and cannot be edited.";
                return View("Error");
            }

            var user = await GetLoggedInUserAsync();
            if (!await _petService.IsUserOwnerOfPetAsync(id, user.Id))
            {
                ViewBag.ErrorMessage = "You are not authorized to edit this pet.";
                return View("Error");
            }
            ViewData["Species"] = new List<Species> { Species.Dog, Species.Cat, Species.Hamster, Species.Rabbit };
            return View(pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pet updatedPet)
        {
            var loginRedirect = RedirectToLoginIfNotLoggedIn();
            if (loginRedirect != null) return loginRedirect;

            var user = await GetLoggedInUserAsync();
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                ViewBag.ErrorMessage = "Pet not found.";
                return View("Error");
            }

            if (!await _petService.IsUserOwnerOfPetAsync(id, user.Id))
            {
                ViewBag.ErrorMessage = "You are not authorized to edit this pet.";
                return View("Error");
            }

            await _petService.UpdatePetAsync(id, updatedPet, user.Id);

            var adoptionRequests = await _adoptionRequestService.GetAdoptionRequestsByPetIdAsync(id);
            foreach (var request in adoptionRequests)
            {
                await SendPetUpdateEmailAsync(request.User, pet);
            }

            return RedirectToAction("Details", new { id = pet.Id });
        }

        public async Task<IActionResult> Delete(int id) {
            var loginRedirect = RedirectToLoginIfNotLoggedIn();
            if (loginRedirect != null) return loginRedirect;

            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                ViewBag.ErrorMessage = "Pet not found.";
                return View("Error");
            }

            var adoption = await _adoptionService.GetAdoptionByPetIdAsync(id);
            if (adoption != null)
            {
                ViewBag.ErrorMessage = "This pet has already been adopted and cannot be deleted.";
                return View("Error");
            }

            var user = await GetLoggedInUserAsync();
            if (!await _petService.IsUserOwnerOfPetAsync(id, user.Id))
            {
                ViewBag.ErrorMessage = "You are not authorized to delete this pet.";
                return View("Error");
            }

            return View(pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loginRedirect = RedirectToLoginIfNotLoggedIn();
            if (loginRedirect != null) return loginRedirect;

            var user = await GetLoggedInUserAsync();
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                ViewBag.ErrorMessage = "Pet not found.";
                return View("Error");
            }

            var adoptionRequests = await _adoptionRequestService.GetAdoptionRequestsByPetIdAsync(id);
            var adoption = await _adoptionService.GetAdoptionByPetIdAsync(id);
            if (adoption != null)
            {
                ViewBag.ErrorMessage = "This pet has already been adopted and cannot be deleted.";
                return View("Error");
            }

            try
            {
                await _petService.DeletePetAsync(id, user.Id);

                foreach (var request in adoptionRequests)
                {
                    await SendPetDeletionEmailAsync(request.User, pet);
                }

                return RedirectToAction("Index", "Adoption");
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.ErrorMessage = "You are not authorized to delete this pet.";
                return View("Error");
            }
            catch (KeyNotFoundException)
            {
                ViewBag.ErrorMessage = "The pet you're trying to delete does not exist.";
                return View("Error");
            }
        }

<<<<<<< HEAD
=======

>>>>>>> bff0f066ccf0a90dbdb0c11703c71d5d43668258
        private async Task SendPetUpdateEmailAsync(User user, Pet pet)
        {
            var subject = "The pet you requested adoption for has been updated";
            var body = EmailHelper.GeneratePetUpdateEmailBody(user, pet);
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        private async Task SendPetDeletionEmailAsync(User user, Pet pet)
        {
            var subject = "The pet you requested adoption for has been deleted";
            var body = EmailHelper.GeneratePetDeletionEmailBody(user, pet);
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
    }
}
