﻿using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Data.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Speices Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<AdoptionRequest> AdoptionRequests { get; set; } = new List<AdoptionRequest>();
        public ICollection<PetOwner> PetOwners { get; set; } = new List<PetOwner>();
    }
}
