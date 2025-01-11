using Microsoft.AspNetCore.Mvc;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.Data.Helpers;

namespace PetMe.Web.Controllers
{
    public class HelpRequestController : Controller
    {
        private readonly IHelpRequestService _helpRequestService;
        private readonly IUserService _userService;
        private readonly IVeterinarianService _veterinarianService;
        private readonly IEmailService _emailService;
        private readonly ICommentService _commentService;

        public HelpRequestController(IHelpRequestService helpRequestService, IUserService userService, IVeterinarianService veterinarianService, IEmailService emailService, ICommentService commentService)
        {
            _helpRequestService = helpRequestService;
            _userService = userService;
            _veterinarianService = veterinarianService;
            _emailService = emailService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var helpRequests = await _helpRequestService.GetHelpRequestsAsync();
            return View(helpRequests);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var helpRequest = new HelpRequest();
            ViewBag.User = user;

            return View(helpRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HelpRequest helpRequest)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            helpRequest.UserId = user.Id;
            helpRequest.CreatedAt = DateTime.UtcNow;
            helpRequest.Status = HelpRequestStatus.Active;

            if (ModelState.IsValid)
            {
                await _helpRequestService.CreateHelpRequestAsync(helpRequest);

                var veterinarians = await _veterinarianService.GetAllVeterinariansAsync();
                foreach (var veterinarian in veterinarians)
                {
                    await SendNewHelpRequestEmailAsync(veterinarian.User, helpRequest, user);
                }

                return RedirectToAction("Index");
            }

            return View(helpRequest);
        }

        private async Task SendNewHelpRequestEmailAsync(User veterinarian, HelpRequest helpRequest, User requester)
        {
            string subject = "New Help Request: Animal in Need!";
            var emailHelper = new EmailHelper();
            string body = EmailHelper.GenerateVeterinarianNotificationEmailBody(helpRequest, requester);

            await _emailService.SendEmailAsync(veterinarian.Email, subject, body);
        }

        private async Task SendUpdatedHelpRequestEmailAsync(User veterinarian, HelpRequest helpRequest, User requester)
        {
            string subject = "Help Request Updated: Animal in Need!";
            var emailHelper = new EmailHelper();
            string body = EmailHelper.GenerateEditHelpRequestEmailBody(helpRequest, requester);

            await _emailService.SendEmailAsync(veterinarian.Email, subject, body);
        }

        

    }
}
