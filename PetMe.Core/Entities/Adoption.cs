using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Data.Entities
{
    public class Adoption
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int UserId { get; set; }
        public DateTime AdoptionDate { get; set; }
        public AdoptionStatus Status { get; set; }
        public Pet Pet { get; set; }
        public User User { get; set; }
    }
}
