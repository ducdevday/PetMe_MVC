using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Core.Entities
{
    public class PetOwner
    {
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime OwnershipDate { get; set; }
    }
}
