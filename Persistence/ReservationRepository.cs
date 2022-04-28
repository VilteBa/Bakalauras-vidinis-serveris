using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Persistence
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DBContext _dbContext;

        public ReservationRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateReservation(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        public void DeleteReservation(Guid id)
        {
            var reservation = _dbContext.Reservations.SingleOrDefault(reservation => reservation.ReservationId == id);
            _dbContext.Reservations.Remove(reservation ?? throw new InvalidOperationException());
            _dbContext.SaveChanges();
        }

        public IEnumerable<Reservation> GetShelterReservations(Guid shelterId)
        {
            return _dbContext.Reservations.Where(r => r.ShelterId == shelterId);
        }

        public IEnumerable<Reservation> GetUserReservations(Guid userId)
        {
            return _dbContext.Reservations.Where(r => r.UserId == userId);

        }
    }
}
