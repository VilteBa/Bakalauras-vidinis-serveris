using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Reservation
    {
        [Key]
        public Guid ReservationId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("Shelter")]
        public Guid ShelterId { get; set; }

        [JsonIgnore]
        public Shelter Shelter { get; set; }

        public DateTime StartTime{ get; set; }

        public DateTime EndTime { get; set; }

    }
}
