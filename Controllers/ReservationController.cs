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

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(Guid id)
        {
            _reservationRepository.DeleteReservation(id);
            return Ok();
        }

        [HttpGet()]
        public List<Reservation> GetReservations([FromQuery] ReservationQueryModel reservationQueryModel)
        {
            return _reservationRepository.GetReservations(reservationQueryModel).ToList();
        }

        [HttpGet("Count")]
        public IActionResult GetCount([FromQuery] ReservationQueryModel reservationQueryModel)
        {
            return Ok(_reservationRepository.CountReservations(reservationQueryModel));
        }
    }
}
