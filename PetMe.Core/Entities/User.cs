﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PetMe.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string ProfileImageUrl { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public string? City { get; set; }
        public string? District { get; set; }

        public ICollection<PetOwner> PetOwners { get; set; } = new List<PetOwner>();
        public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
        public ICollection<HelpRequest> HelpRequests { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
