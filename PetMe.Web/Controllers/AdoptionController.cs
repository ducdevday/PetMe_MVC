using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.Data.Helpers;
using PetMe.DataAccess.Repositories;

namespace PetMe.Web.Controllers
{
    public class AdoptionController : Controller
    {
        private readonly IAdoptionService _adoptionService;
        private readonly IPetService _petService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IPetOwnerService _petOwnerService;
        private readonly IAdoptionRequestService _adoptionRequestService;

        public AdoptionController(
        IAdoptionService adoptionService,
        IAdoptionRequestService adoptionRequestService,
        IPetService petService,
        IUserService userService,
        IEmailService emailService,
        IPetOwnerService petOwnerService)
        {
            _adoptionService = adoptionService;
            _petService = petService;
            _userService = userService;
            _emailService = emailService;
            _petOwnerService = petOwnerService;
            _adoptionRequestService = adoptionRequestService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pets = await _petService.GetAllPetsAsync();
            return View(pets);
        }

        public async Task<IActionResult> Adopt(int petId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var existingRequest = await _adoptionRequestService.GetAdoptionRequestByUserAndPetAsync(user.Id, petId);
            if (existingRequest != null)
            {
                ViewBag.ErrorMessage = "You have already submitted an adoption request for this pet.";
                ViewBag.PetId = petId;
                return View("AdoptionRequestExists");
            }

            var pet = await _petService.GetPetByIdAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }

            ViewData["PetName"] = pet.Name;
            ViewData["PetId"] = pet.Id;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Adopt(int petId, string name, string email, string phone, string address, DateTime dateOfBirth, string message)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var pet = await _petService.GetPetByIdAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var existingRequest = await _adoptionRequestService.GetAdoptionRequestByUserAndPetAsync(user.Id, petId);
            if (existingRequest != null)
            {
                ViewBag.ErrorMessage = "You have already submitted an adoption request for this pet.";
                ViewBag.PetId = petId;
                ViewBag.PetName = pet.Name;
                return View("AdoptionRequestExists");
            }

            var adoptionRequest = new AdoptionRequest
            {
                PetId = petId,
                Message = message,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
                UserId = user.Id,
            };

            await _adoptionRequestService.CreateAdoptionRequestAsync(adoptionRequest);
            await SendAdoptionRequestNotificationAsync(adoptionRequest);
            await SendAdoptionConfirmationEmailAsync(user, pet);

            return RedirectToAction("Details", "Pet", new { id = petId });
        }

        public async Task<IActionResult> ApproveRequest(int adoptionRequestId, int petId)
        {
            var adoptionRequest = await _adoptionRequestService.GetAdoptionRequestByIdAsync(adoptionRequestId);
            if (adoptionRequest == null || adoptionRequest.PetId != petId)
            {
                return NotFound();
            }

            var pet = await _petService.GetPetByIdAsync(petId);
            var petOwner = pet.PetOwners.FirstOrDefault();

            if (petOwner?.UserId.ToString() != User?.Identity?.Name)
            {
                return Unauthorized();
            }

            adoptionRequest.Status = AdoptionStatus.Approved;
            await _adoptionRequestService.UpdateAdoptionRequestAsync(adoptionRequest);

            var approvedUser = adoptionRequest.User;
            if (approvedUser != null)
            {
                await SendApprovalEmailAsync(approvedUser, pet);
            }

            var pendingRequests = await _adoptionRequestService.GetPendingRequestsByPetIdAsync(petId);
            if (pendingRequests != null)
            {
                foreach (var request in pendingRequests)
                {
                    if (request.Id != adoptionRequestId)
                    {
                        request.Status = AdoptionStatus.Rejected;
                        await _adoptionRequestService.UpdateAdoptionRequestAsync(request);

                        var rejectedUser = request.User;
                        if (rejectedUser != null)
                        {
                            await SendRejectionEmailAsync(rejectedUser, pet);
                        }
                    }
                }
            }

            var adoption = new Adoption
            {
                PetId = petId,
                UserId = adoptionRequest.UserId,
                AdoptionDate = DateTime.UtcNow,
                Status = AdoptionStatus.Approved,
                Pet = pet,
                User = adoptionRequest.User
            };

            await _adoptionService.CreateAdoptionAsync(adoption);

            return RedirectToAction("Index");
        }


        public async Task SendAdoptionRequestNotificationAsync(AdoptionRequest adoptionRequest)
        {
            var petOwner = await _petOwnerService.GetPetOwnerByPetIdAsync(adoptionRequest.PetId);
            if (petOwner == null)
            {
                return;
            }

            var petOwnerUser = await _userService.GetUserByIdAsync(petOwner.UserId);

            var user = adoptionRequest.User;
            var pet = adoptionRequest.Pet;

            var subject = "New Adoption Request for Your Pet";
            var body = EmailHelper.GenerateAdoptionRequestEmailBody(user, pet, adoptionRequest);

            await _emailService.SendEmailAsync(petOwnerUser.Email, subject, body);
        }

        public async Task SendAdoptionConfirmationEmailAsync(User user, Pet pet)
        {
            var subject = "Adoption Request Submitted Successfully";
            var body = EmailHelper.GenerateAdoptionRequestConfirmationEmailBody(user, pet);

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        private async Task SendApprovalEmailAsync(User user, Pet pet)
        {
            var subject = "Your Adoption Request Has Been Approved";
            var body = EmailHelper.GenerateAdoptionConfirmationEmailBody(user, pet);
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        private async Task SendRejectionEmailAsync(User user, Pet pet)
        {
            var subject = "Adoption Request Rejected";
            var body = EmailHelper.GenerateRejectionEmailBody(user, pet);
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

    }
}
