using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class File
    {
        [Key]
        public Guid FileId { get; set; }

        public string FileName { get; set; }

        public byte[] Data { get; set; }

        [ForeignKey("Shelter")]
        public Guid? ShelterId { get; set; }

        [JsonIgnore]
        public Shelter Shelter { get; set; }

        [ForeignKey("Pet")]
        public Guid? PetId { get; set; }

        [JsonIgnore]
        public Pet Pet { get; set; }
    }
}
