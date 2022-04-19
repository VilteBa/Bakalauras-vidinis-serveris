using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class LovedPets
    {
        //[Key]
        public Guid PetId { get; set; }

        public Pet Pet { get; set; }

        //[Key]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
