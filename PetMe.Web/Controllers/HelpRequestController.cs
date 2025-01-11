﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var helpRequest = await _helpRequestService.GetHelpRequestByIdAsync(id);

            if (helpRequest == null)
            {
                return NotFound();
            }

            var comments = await _commentService.GetCommentsByHelpRequestIdAsync(id);
            helpRequest.Comments = comments;

            var username = HttpContext.Session.GetString("Username");
            var user = username != null ? await _userService.GetUserByUsernameAsync(username) : null;

            ViewBag.CanEditOrDelete = user != null && helpRequest.UserId == user.Id;
            ViewBag.isVeterinarian = user != null && await _veterinarianService.GetApprovedByUserIdAsync(user.Id) != null;
            if (user != null)
            {
                ViewBag.CanEditOrDeleteComment = comments.Where(c => c.UserId == user.Id).Select(c => c.Id).ToList();
            }
            return View(helpRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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

            var helpRequest = await _helpRequestService.GetHelpRequestByIdAsync(id);
            if (helpRequest == null || helpRequest.UserId != user.Id)
            {
                return Unauthorized();
            }

            return View(helpRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HelpRequest helpRequest)
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

            var existingRequest = await _helpRequestService.GetHelpRequestByIdAsync(helpRequest.Id);
            if (existingRequest == null || existingRequest.UserId != user.Id)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                existingRequest.Title = helpRequest.Title;
                existingRequest.Description = helpRequest.Description;
                existingRequest.EmergencyLevel = helpRequest.EmergencyLevel;
                existingRequest.Status = helpRequest.Status;
                existingRequest.Location = helpRequest.Location;
                existingRequest.ContactName = helpRequest.ContactName;
                existingRequest.ContactPhone = helpRequest.ContactPhone;
                existingRequest.ContactEmail = helpRequest.ContactEmail;
                existingRequest.ImageUrl = helpRequest.ImageUrl;

                await _helpRequestService.UpdateHelpRequestAsync(existingRequest);

                var veterinarians = await _veterinarianService.GetAllVeterinariansAsync();
                foreach (var vet in veterinarians)
                {
                    await SendUpdatedHelpRequestEmailAsync(vet.User, existingRequest, user);
                }

                return RedirectToAction("Index");
            }

            return View(helpRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
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

            var helpRequest = await _helpRequestService.GetHelpRequestByIdAsync(id);
            if (helpRequest == null || helpRequest.UserId != user.Id)
            {
                return Unauthorized();
            }

            await _helpRequestService.DeleteHelpRequestAsync(id);

            var veterinarians = await _veterinarianService.GetAllVeterinariansAsync();
            foreach (var veterinarian in veterinarians)
            {
                await SendDeletedHelpRequestEmailAsync(veterinarian.User, helpRequest, user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, string content)
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

            var helpRequest = await _helpRequestService.GetHelpRequestByIdAsync(id);
            if (helpRequest == null)
            {
                return NotFound();
            }

            // Check if the user is a veterinarian
            var veterinarian = await _veterinarianService.GetByUserIdAsync(user.Id);
            int? veterinarianId = veterinarian?.Id; // If user is a veterinarian, get their ID, otherwise set to null

            var comment = new Comment
            {
                Content = content,
                CreatedAt = DateTime.UtcNow,
                HelpRequestId = helpRequest.Id,
                UserId = user.Id,
                VeterinarianId = veterinarianId // Store VeterinarianId if the user is a veterinarian
            };

            // Add the comment to the database
            await _commentService.AddCommentAsync(comment);

            return RedirectToAction("Details", "HelpRequest", new { id = helpRequest.Id });
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null || comment.UserId != user.Id)
            {
                return Unauthorized();
            }

            return View(comment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, int helpRequestId, string content)
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

            var existingComment = await _commentService.GetCommentByIdAsync(id);
            if (existingComment == null || existingComment.UserId != user.Id)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError(nameof(content), "Content cannot be empty.");
                return View(existingComment); // Hatalı durumlarda eski yorumu yeniden yükler.
            }

            existingComment.Content = content;
            existingComment.CreatedAt = DateTime.UtcNow;

            var veterinarian = await _veterinarianService.GetByUserIdAsync(user.Id);
            existingComment.VeterinarianId = veterinarian?.Id;

            await _commentService.UpdateCommentAsync(existingComment);

            return RedirectToAction("Details", new { id = helpRequestId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int commentId)
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

            var comment = await _commentService.GetCommentByIdAsync(commentId);
            if (comment == null || comment.UserId != user.Id)
            {
                return Unauthorized();
            }

            await _commentService.DeleteCommentAsync(commentId);

            return RedirectToAction("Details", new { id = comment.HelpRequestId });
        }

        private async Task SendNewHelpRequestEmailAsync(User veterinarian, HelpRequest helpRequest, User requester)
        {
            string subject = "New Help Request: Animal in Need!";
            string body = EmailHelper.GenerateVeterinarianNotificationEmailBody(helpRequest, requester);

            await _emailService.SendEmailAsync(veterinarian.Email, subject, body);
        }

        private async Task SendUpdatedHelpRequestEmailAsync(User veterinarian, HelpRequest helpRequest, User requester)
        {
            string subject = "Help Request Updated: Animal in Need!";
            string body = EmailHelper.GenerateEditHelpRequestEmailBody(helpRequest, requester);

            await _emailService.SendEmailAsync(veterinarian.Email, subject, body);
        }

        private async Task SendDeletedHelpRequestEmailAsync(User veterinarian, HelpRequest helpRequest, User requester)
        {
            string subject = "Help Request Deleted: Animal in Need!";
            string body = EmailHelper.GenerateDeleteHelpRequestEmailBody(helpRequest, requester);

            await _emailService.SendEmailAsync(veterinarian.Email, subject, body);
        }


    }
}
