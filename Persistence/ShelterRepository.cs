using backend.Controllers;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Persistence
{
    public class ShelterRepository: IShelterRepository
    {
        private readonly DBContext _dbContext;

        public ShelterRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Shelter> GetShelters(SheltersQueryModel sheltersQueryModel)
        {
            return _dbContext.Shelters.AsQueryable()
                .FilterByCity(sheltersQueryModel.Cities)
                .Skip(sheltersQueryModel.Page * sheltersQueryModel.PageLimit)
                .Take(sheltersQueryModel.PageLimit);
        }

        Shelter IShelterRepository.GetShelter(Guid id)
        {
            return _dbContext.Shelters.Include(s => s.Pets).SingleOrDefault(shelter => shelter.ShelterId == id);
        }

        public void CreateShelter(Shelter shelter)
        {//todo: if not exists
            _dbContext.Shelters.Add(shelter);
            _dbContext.SaveChanges();
        }

        public void DeleteShelter(Guid id)
        {
            var shelter = _dbContext.Shelters.SingleOrDefault(shelter => shelter.ShelterId == id);
            _dbContext.Shelters.Remove(shelter ?? throw new InvalidOperationException());
            _dbContext.SaveChanges();
        }

        public void UpdateShelter(Shelter shelter)
        {
            var shelterToUpdate = _dbContext.Shelters.SingleOrDefault(p => p.ShelterId == shelter.ShelterId);
            _dbContext.Entry(shelterToUpdate).CurrentValues.SetValues(shelter);
            _dbContext.SaveChanges();
        }
    }
}
