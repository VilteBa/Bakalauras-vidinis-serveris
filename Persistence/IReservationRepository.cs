using backend.Controllers;
using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.Persistence
{
    public interface IReservationRepository
    {
        void CreateReservation(Reservation reservation);
        void DeleteReservation(Guid id);
        IEnumerable<Reservation> GetUserReservations(Guid userId);
        IEnumerable<Reservation> GetShelterReservations(Guid shelterId);
        IEnumerable<Reservation> GetReservations(ReservationQueryModel reservationQueryModel);
        int CountReservations(ReservationQueryModel reservationQueryModel);
    }
}