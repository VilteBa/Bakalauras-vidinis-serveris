using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Pet
    {
        [Key]
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public Size Size { get; set; }
        public Color Color { get; set; }
        public Sex Sex { get; set; }
        public PetType Type { get; set; }

        [ForeignKey("Shelter")]//gal nereikia visu atributu?
        public Guid ShelterId { get; set; }

        //navigation prop
        public virtual Shelter Shelter { get; set; }
        public virtual ICollection<User> Users { get; set; }

        [JsonIgnore]
        public virtual ICollection<LovedPets> LovedPets { get; set; }
    }
}
