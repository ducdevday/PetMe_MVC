using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int HelpRequestId { get; set; }
        public HelpRequest HelpRequest { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? VeterinarianId { get; set; }
        public Veterinarian Veterinarian { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
