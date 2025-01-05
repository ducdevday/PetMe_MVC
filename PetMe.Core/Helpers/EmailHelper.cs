﻿using PetMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Data.Helpers
{
    public class EmailHelper
    {
        private const string CssLink = "<link rel='stylesheet' type='text/css' href='https://yourdomain.com/path/to/email.css'>";

<<<<<<< HEAD
        public static string GenerateAdoptionRequestEmailBody(User user, Pet pet, AdoptionRequest adoptionRequest)
=======
        public string GenerateAdoptionRequestEmailBody(User user, Pet pet, AdoptionRequest adoptionRequest)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>New Adoption Request for Your Pet: {pet.Name}</h2>
                <p class='info'><strong>Requested by:</strong> {user.Username}</p>
                <p class='info'><strong>Message from the adopter:</strong> {adoptionRequest.Message}</p>
                <p class='status'><strong>Status:</strong> {adoptionRequest.Status}</p>

                <div class='divider'></div>

                <h3 class='section-header'>Pet Details:</h3>
                <ul class='details-list'>
                    <li><strong>Name:</strong> {pet.Name}</li>
                    <li><strong>Species:</strong> {pet.Species}</li>
                    <li><strong>Breed:</strong> {pet.Breed}</li>
                    <li><strong>Age:</strong> {pet.Age} years old</li>
                    <li><strong>Gender:</strong> {pet.Gender}</li>
                    <li><strong>Weight:</strong> {pet.Weight} kg</li>
                    <li><strong>Color:</strong> {pet.Color}</li>
                    <li><strong>Vaccination Status:</strong> {pet.VaccinationStatus}</li>
                    <li><strong>Microchip ID:</strong> {pet.MicrochipId}</li>
                    <li><strong>Is Neutered:</strong> {(pet.IsNeutered.HasValue ? (pet.IsNeutered.Value ? "Yes" : "No") : "Not specified")}</li>
                </ul>

                <div class='divider'></div>

                <h3 class='section-header'>User Details:</h3>
                <ul class='details-list'>
                    <li><strong>Name:</strong> {user.Username}</li>
                    <li><strong>Email:</strong> {user.Email}</li>
                    <li><strong>Phone:</strong> {user.PhoneNumber}</li>
                    <li><strong>Address:</strong> {user.Address}</li>
                    <li><strong>Date of Birth:</strong> {user.DateOfBirth.ToString("yyyy-MM-dd")}</li>
                    <li><strong>Account Created:</strong> {user.CreatedDate.ToString("yyyy-MM-dd")}</li>
                </ul>

                <div class='divider'></div>

                <p>If you wish to review or respond to this adoption request, please log into your account.</p>
                <a href='#' class='btn-primary'>Review Adoption Request</a>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GenerateAdoptionRequestConfirmationEmailBody(User user, Pet pet)
=======
        public string GenerateAdoptionRequestConfirmationEmailBody(User user, Pet pet)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Thank you for your Adoption Request!</h2>
                <p>Dear {user.Username},</p>
                <p>Thank you for your adoption request for {pet.Name}. Your request has been successfully submitted and is currently under review.</p>
                <p>We will notify you once your request has been processed.</p>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GenerateRejectionEmailBody(User user, Pet pet)
=======
        public string GenerateRejectionEmailBody(User user, Pet pet)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Your Adoption Request for {pet.Name} Has Been Rejected</h2>
                <p>Dear {user.Username},</p>
                <p>We regret to inform you that your adoption request for {pet.Name} has been rejected. Unfortunately, another user’s request for this pet has been approved.</p>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GenerateAdoptionConfirmationEmailBody(User user, Pet pet)
=======
        public string GenerateAdoptionConfirmationEmailBody(User user, Pet pet)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Your Adoption Request for {pet.Name} Has Been Approved</h2>
                <p>Dear {user.Username},</p>
                <p>We are pleased to inform you that your adoption request for <span class='highlight'>{pet.Name}</span> has been approved!</p>
                <p>Thank you for choosing to adopt, and we hope you and {pet.Name} will have a wonderful time together.</p>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GeneratePetDeletionEmailBody(User user, Pet pet)
=======
        public string GeneratePetDeletionEmailBody(User user, Pet pet)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Pet Removed: {pet.Name}</h2>
                <p>Dear {user.Username},</p>
                <p>We regret to inform you that the pet <span class='highlight'>{pet.Name}</span> you were interested in has been removed from our platform. It is no longer available for adoption.</p>
                <p>We apologize for any inconvenience this may have caused. Please feel free to explore other available pets.</p>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GeneratePetUpdateEmailBody(User user, Pet pet)
=======
        public string GeneratePetUpdateEmailBody(User user, Pet pet)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Pet Information Updated: {pet.Name}</h2>
                <p>Dear {user.Username},</p>
                <p>The details of the pet you were interested in, <span class='highlight'>{pet.Name}</span>, have been updated. Please review the updated information.</p>
                <p>Here are the updated details:</p>
                <ul class='details-list'>
                    <li><strong>Name:</strong> {pet.Name}</li>
                    <li><strong>Breed:</strong> {pet.Breed}</li>
                    <li><strong>Age:</strong> {pet.Age} years old</li>
                </ul>
                <p>Please log into your account to view the full details.</p>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GeneratePetCreationEmailBody(User user, Pet pet)
=======
        public string GeneratePetCreationEmailBody(User user, Pet pet)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Your Pet {pet.Name} Has Been Successfully Created</h2>
                <p>Dear {user.Username},</p>
                <p>Thank you for creating a profile for your pet, <span class='highlight'>{pet.Name}</span>. The pet has been successfully added to our system.</p>
                <p>You can now view and manage the pet's details from your profile.</p>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }


<<<<<<< HEAD
        public static string GenerateVeterinarianNotificationEmailBody(HelpRequest helpRequest, User requester)
=======
        public string GenerateVeterinarianNotificationEmailBody(HelpRequest helpRequest, User requester)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>New Help Request for an Animal in Need!</h2>
                <p>Dear Veterinarian,</p>
                <p>A new help request has been created for an animal requiring immediate attention:</p>
                <ul class='details-list'>
                    <li><strong>Description:</strong> {helpRequest.Description}</li>
                    <li><strong>Emergency Level:</strong> {helpRequest.EmergencyLevel}</li>
                    <li><strong>Created At:</strong> {helpRequest.CreatedAt.ToString("yyyy-MM-dd HH:mm")}</li>
                    <li><strong>Requested By:</strong> {requester.Username} ({requester.Email})</li>
                </ul>

                <p>Please log in to the system to review this request in detail and provide assistance.</p>
                <a href='https://yourdomain.com/HelpRequest/Details/{helpRequest.Id}' class='btn-primary'>View Help Request</a>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GenerateCreateHelpRequestEmailBody(HelpRequest helpRequest, User requester)
=======
        // Create HelpRequest Email Body
        public string GenerateCreateHelpRequestEmailBody(HelpRequest helpRequest, User requester)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>New Help Request Created for an Animal in Need!</h2>
                <p>Dear Veterinarian,</p>
                <p>A new help request has been created for an animal requiring immediate attention:</p>
                <ul class='details-list'>
                    <li><strong>Description:</strong> {helpRequest.Description}</li>
                    <li><strong>Emergency Level:</strong> {helpRequest.EmergencyLevel}</li>
                    <li><strong>Created At:</strong> {helpRequest.CreatedAt.ToString("yyyy-MM-dd HH:mm")}</li>
                    <li><strong>Requested By:</strong> {requester.Username} ({requester.Email})</li>
                </ul>

                <p>Please log in to the system to review this request in detail and provide assistance.</p>
                <a href='https://yourdomain.com/HelpRequest/Details/{helpRequest.Id}' class='btn-primary'>View Help Request</a>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GenerateEditHelpRequestEmailBody(HelpRequest helpRequest, User requester)
=======
        // Edit HelpRequest Email Body
        public string GenerateEditHelpRequestEmailBody(HelpRequest helpRequest, User requester)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Help Request Updated for an Animal in Need!</h2>
                <p>Dear Veterinarian,</p>
                <p>The following help request has been updated:</p>
                <ul class='details-list'>
                    <li><strong>Description:</strong> {helpRequest.Description}</li>
                    <li><strong>Emergency Level:</strong> {helpRequest.EmergencyLevel}</li>
                    <li><strong>Updated At:</strong> {helpRequest.CreatedAt.ToString("yyyy-MM-dd HH:mm")}</li>
                    <li><strong>Requested By:</strong> {requester.Username} ({requester.Email})</li>
                </ul>

                <p>Please log in to the system to review the updated request in detail.</p>
                <a href='https://yourdomain.com/HelpRequest/Details/{helpRequest.Id}' class='btn-primary'>View Updated Help Request</a>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }

<<<<<<< HEAD
        public static string GenerateDeleteHelpRequestEmailBody(HelpRequest helpRequest, User requester)
=======
        // Delete HelpRequest Email Body
        public string GenerateDeleteHelpRequestEmailBody(HelpRequest helpRequest, User requester)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
        <html>
        <head>
            {CssLink}
        </head>
        <body>
            <div class='email-container'>
                <h2 class='header'>Help Request Deleted for an Animal in Need!</h2>
                <p>Dear Veterinarian,</p>
                <p>The following help request has been deleted:</p>
                <ul class='details-list'>
                    <li><strong>Description:</strong> {helpRequest.Description}</li>
                    <li><strong>Emergency Level:</strong> {helpRequest.EmergencyLevel}</li>
                    <li><strong>Deleted At:</strong> {helpRequest.CreatedAt.ToString("yyyy-MM-dd HH:mm")}</li>
                    <li><strong>Requested By:</strong> {requester.Username} ({requester.Email})</li>
                </ul>

                <p>Please note that this request is no longer available for review.</p>

                <div class='footer'>
                    <p>Best regards,</p>
                    <p>The PetSoLive Team</p>
                </div>
            </div>
        </body>
        </html>";
        }
<<<<<<< HEAD

        public static string GenerateNewCommentEmailBody(HelpRequest helpRequest, Comment comment, User commenter)
=======
        // Yeni yorum için e-posta gövdesi oluşturma
        public string GenerateNewCommentEmailBody(HelpRequest helpRequest, Comment comment, User commenter)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return "<h2>New Comment on Help Request: Animal in Need!</h2>" +
                   "<p><strong>Help Request Title:</strong> " + helpRequest.Title + "</p>" +
                   "<p><strong>Description:</strong> " + helpRequest.Description + "</p>" +
                   "<p><strong>Comment by:</strong> " + commenter.Username + "</p>" +
                   "<p><strong>Comment Content:</strong> " + comment.Content + "</p>" +
                   "<p><strong>Comment Date:</strong> " + comment.CreatedAt.ToString("dd MMM yyyy, hh:mm tt") + "</p>" +
                   "<p><strong>Location:</strong> " + helpRequest.Location + "</p>" +
                   "<br>" +
                   "<p>Please check the help request and the comment left by the user.</p>" +
                   "<p><a href='" + "/HelpRequest/Details/" + helpRequest.Id + "'>Click here to view the help request</a></p>" +
                   "<br>" +
                   "<p>Thank you for your attention.</p>";
        }

<<<<<<< HEAD
        public static string GenerateNewLostPetAdEmailBody(LostPetAd lostPetAd, User user)
=======
        // Kayıp ilanı için yeni e-posta içeriği oluşturuluyor
        public string GenerateNewLostPetAdEmailBody(LostPetAd lostPetAd, User user)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
            A new lost pet ad has been posted.
            Pet Name: {lostPetAd.PetName}
            Location: {lostPetAd.LastSeenCity}, {lostPetAd.LastSeenDistrict}
            Description: {lostPetAd.Description}
            Posted by: {user.Username} ({user.Email})
            Contact: {user.PhoneNumber}
        ";
        }

<<<<<<< HEAD
        public static string GenerateUpdatedLostPetAdEmailBody(LostPetAd lostPetAd, User user)
=======
        // Kayıp ilanı güncellenince gönderilecek e-posta içeriği
        public string GenerateUpdatedLostPetAdEmailBody(LostPetAd lostPetAd, User user)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
            The lost pet ad has been updated.
            Pet Name: {lostPetAd.PetName}
            Location: {lostPetAd.LastSeenCity}, {lostPetAd.LastSeenDistrict}
            Description: {lostPetAd.Description}
            Posted by: {user.Username} ({user.Email})
            Contact: {user.PhoneNumber}
        ";
        }

<<<<<<< HEAD
        public static string GenerateDeletedLostPetAdEmailBody(LostPetAd lostPetAd, User user)
=======
        // Kayıp ilanı silindiğinde gönderilecek e-posta içeriği
        public string GenerateDeletedLostPetAdEmailBody(LostPetAd lostPetAd, User user)
>>>>>>> 3d3110e53285da6bf21caa8edb3aa016a72d2768
        {
            return $@"
            A lost pet ad has been deleted.
            Pet Name: {lostPetAd.PetName}
            Location: {lostPetAd.LastSeenCity}, {lostPetAd.LastSeenDistrict}
            Description: {lostPetAd.Description}
            Posted by: {user.Username} ({user.Email})
            Contact: {user.PhoneNumber}
        ";
        }
    }
}
