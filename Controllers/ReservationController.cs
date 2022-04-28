using backend.Models;
using backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        [HttpPost]
        public IActionResult CreateShelter([FromBody] Reservation reservation)
        {
            _reservationRepository.CreateReservation(reservation);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShelter(Guid id)
        {
            _reservationRepository.DeleteReservation(id);
            return Ok();
        }

        [HttpGet("User/{id}")]
        public List<Reservation> GetUserReservations(Guid id)
        {
            return _reservationRepository.GetUserReservations(id).ToList();
        }

        [HttpGet("Shelter/{id}")]
        public List<Reservation> GetShelterReservations(Guid id)
        {
            return _reservationRepository.GetShelterReservations(id).ToList();
        }
    }
}
