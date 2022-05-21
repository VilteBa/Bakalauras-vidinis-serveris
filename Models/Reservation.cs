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

        public User User { get; set; }

        [ForeignKey("Shelter")]
        public Guid ShelterId { get; set; }

        public Shelter Shelter { get; set; }

        public DateTimeOffset StartTime{ get; set; }

        public DateTimeOffset EndTime { get; set; }

        public ReservationState ReservationState { get; set; }

    }
}
