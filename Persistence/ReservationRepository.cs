using backend.Controllers;
using backend.Models;
using Microsoft.EntityFrameworkCore;
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
            reservation.ReservationState = ReservationState.None;
            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        public void DeleteReservation(Guid id)
        {
            var reservation = _dbContext.Reservations.SingleOrDefault(reservation => reservation.ReservationId == id);
            _dbContext.Reservations.Remove(reservation ?? throw new InvalidOperationException());
            _dbContext.SaveChanges();
        }

        public IEnumerable<Reservation> GetReservations(ReservationQueryModel reservationQueryModel)
        {
            return _dbContext.Reservations
                .Include(r => r.Shelter)
                .Include(r => r.User)
                .FilterByUser(reservationQueryModel.UserId)
                .FilterByShelter(reservationQueryModel.ShelterId)
                .FilterByDate(reservationQueryModel.StartTime, reservationQueryModel.EndTime)
                .FilterByState(reservationQueryModel.ReservationState)
                .OrderBy(r => r.StartTime)
                .Skip(reservationQueryModel.Page * reservationQueryModel.PageLimit)
                .Take(reservationQueryModel.PageLimit);
        }

        public IEnumerable<Reservation> GetShelterReservations(Guid shelterId)
        {
            return _dbContext.Reservations.Include(r => r.User).Where(r => r.ShelterId == shelterId).OrderBy(r => r.StartTime);
        }

        public IEnumerable<Reservation> GetUserReservations(Guid userId)
        {
            return _dbContext.Reservations.Include(r => r.Shelter).Where(r => r.UserId == userId).OrderBy(r => r.StartTime);
        }

        public int CountReservations(ReservationQueryModel reservationQueryModel)
        {
            return _dbContext.Reservations
               .Include(r => r.Shelter)
               .Include(r => r.User)
               .FilterByUser(reservationQueryModel.UserId)
               .FilterByShelter(reservationQueryModel.ShelterId)
               .FilterByDate(reservationQueryModel.StartTime, reservationQueryModel.EndTime)
               .Count();
        }

        public void ApproveReservation(Guid id)
        {
            var reservation = _dbContext.Reservations.SingleOrDefault(r => r.ReservationId == id);
            reservation.ReservationState = ReservationState.Approved;
            _dbContext.SaveChanges();
        }

        public void CancelReservation(Guid id)
        {
            var reservation = _dbContext.Reservations.SingleOrDefault(r => r.ReservationId == id);
            reservation.ReservationState = ReservationState.Canceled;
            _dbContext.SaveChanges();
        }
    }
}
