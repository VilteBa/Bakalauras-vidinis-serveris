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
        public IActionResult CreateReservation([FromBody] Reservation reservation)
        {
            _reservationRepository.CreateReservation(reservation);
            return Ok();
        }

        [HttpPut("{id}/approve")]
        public IActionResult ApproveReservation(Guid id)
        {
            _reservationRepository.ApproveReservation(id);
            return Ok();
        }

        [HttpPut("{id}/cancel")]
        public IActionResult CancelReservation(Guid id)
        {
            _reservationRepository.CancelReservation(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(Guid id)
        {
            _reservationRepository.DeleteReservation(id);
            return Ok();
        }

        [HttpGet()]
        public IActionResult GetReservations([FromQuery] ReservationQueryModel reservationQueryModel)
        {
            var reservations = _reservationRepository.GetReservations(reservationQueryModel).ToList();
            if (reservations == null || !reservations.Any())
            {
                return NotFound();
            }
            return Ok(reservations);
        }

        [HttpGet("Count")]
        public IActionResult GetCount([FromQuery] ReservationQueryModel reservationQueryModel)
        {
            return Ok(_reservationRepository.CountReservations(reservationQueryModel));
        }

        [HttpGet("States")]
        public IActionResult GetStates()
        {
            return Ok(Enum.GetValues(typeof(ReservationState)));
        }
    }
}
