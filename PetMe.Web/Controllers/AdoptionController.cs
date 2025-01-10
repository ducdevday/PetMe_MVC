using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;
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


        


    }
}
