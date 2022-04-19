using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace backend.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public Role Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [ForeignKey("Shelter")]
        public Guid? ShelterId { get; set; }//sheleriui priklauso tik darbuotojai

        //navigation prop
        public Shelter Shelter { get; set; }

        [JsonIgnore]//todo: Kad neuzsiloopintu
        public virtual ICollection<Pet> Pets { get; set; }

        [JsonIgnore]
        public virtual ICollection<LovedPets> LovedPets { get; set; }

    }
}
