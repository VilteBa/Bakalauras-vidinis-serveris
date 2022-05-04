using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Shelter
    {
        [Key]
        public Guid ShelterId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }

        //navigation prop
        [JsonIgnore]//todo: Kad neuzsiloopintu
        public virtual ICollection<Pet> Pets { get; set; }// todo: pabandyt be virtual

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }// todo: pabandyt be virtual

        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }

        [JsonIgnore]//todo: Kad neuzsiloopintu
        public virtual File ShelterPhoto { get; set; }
    }
}
