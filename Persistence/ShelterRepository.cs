using backend.Controllers;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Persistence
{
    public class ShelterRepository : IShelterRepository
    {
        private readonly DBContext _dbContext;

        public ShelterRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Shelter> GetShelters(SheltersQueryModel sheltersQueryModel)
        {
            return _dbContext.Shelters
                .Include(s => s.ShelterPhoto)
                .AsQueryable()
                .FilterByCity(sheltersQueryModel.Cities)
                .Skip(sheltersQueryModel.Page * sheltersQueryModel.PageLimit)
                .Take(sheltersQueryModel.PageLimit);
        }

        public int CountShelters(SheltersQueryModel sheltersQueryModel)
        {
            return _dbContext.Shelters.AsQueryable()
                .FilterByCity(sheltersQueryModel.Cities)
                .Count();
        }

        public Shelter GetShelter(Guid id)
        {
            return _dbContext.Shelters
                .Include(s => s.ShelterPhoto)
                .Include(s => s.Pets)
                .SingleOrDefault(shelter => shelter.ShelterId == id);
        }

        public void CreateShelter(Shelter shelter)
        {
            _dbContext.Shelters.Add(shelter);
            _dbContext.SaveChanges();
        }

        public void DeleteShelter(Guid id)
        {
            var shelter = _dbContext.Shelters.SingleOrDefault(shelter => shelter.ShelterId == id);
            _dbContext.Shelters.Remove(shelter ?? throw new InvalidOperationException());
            _dbContext.SaveChanges();
        }
        public IEnumerable<string> GetShelterCities()
        {
            return _dbContext.Shelters.Select(x => x.City).Distinct();
        }

        public void UpdateShelter(Shelter shelter)
        {
            var shelterToUpdate = _dbContext.Shelters.SingleOrDefault(p => p.ShelterId == shelter.ShelterId);
            _dbContext.Entry(shelterToUpdate).CurrentValues.SetValues(shelter);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Pet> GetShelterPets(Guid id, PetsQueryModel petsQueryModel)
        {
            var shelterPets = _dbContext.Pets.Where(p=> p.ShelterId==id);

            return shelterPets
                .Include(s => s.Photos)
                .FilterBySize(petsQueryModel.Sizes)
                .FilterBySex(petsQueryModel.Sexes)
                .FilterByType(petsQueryModel.Types)
                .FilterByColor(petsQueryModel.Colors)
                .FilterByAge(petsQueryModel.MinAge, petsQueryModel.MaxAge)
                .Skip(petsQueryModel.Page * petsQueryModel.PageLimit)
                .Take(petsQueryModel.PageLimit);
        }
        public int CountShelterPets(Guid id, PetsQueryModel petsQueryModel)
        {
            var shelterPets = GetShelter(id).Pets;

            return shelterPets
                .FilterBySize(petsQueryModel.Sizes)
                .FilterBySex(petsQueryModel.Sexes)
                .FilterByType(petsQueryModel.Types)
                .FilterByColor(petsQueryModel.Colors)
                .FilterByAge(petsQueryModel.MinAge, petsQueryModel.MaxAge)
                .Count();
        }

    }
}
