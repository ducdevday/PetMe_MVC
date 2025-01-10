using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Data.Entities
{
    public class LostPetAd
    {
        public int Id { get; set; }
        public string PetName { get; set; }
        public string Description { get; set; }
        public DateTime LastSeenDate { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        // Separate properties for City and District where the pet was last seen
        public string LastSeenCity { get; set; }
        public string LastSeenDistrict { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Optional: Combine both into a single LastSeenLocation for display purposes
        public string LastSeenLocation => $"{LastSeenCity}, {LastSeenDistrict}";
    }
}
