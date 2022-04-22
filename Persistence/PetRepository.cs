using backend.Controllers;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Persistence
{
    public class PetRepository: IPetRepository
    {
        private readonly DBContext _dbContext;

        public PetRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Pet GetPet(Guid petId)
        {
            return _dbContext.Pets.Include(p => p.Shelter).Include(p => p.Users).SingleOrDefault(pet => pet.PetId == petId);
        }

        public void DeletePet(Guid petId)
        {
            var pet = _dbContext.Pets.SingleOrDefault(pet => pet.PetId == petId);
            _dbContext.Pets.Remove(pet ?? throw new InvalidOperationException());
            _dbContext.SaveChanges();

            // todo: isvalyt ir is pamegtu?
        }

        public IEnumerable<Pet> GetPets(PetsQueryModel petsQueryModel)
        {
            var petsFromDB = _dbContext.Pets
                .FilterBySize(petsQueryModel.Sizes)
                .FilterBySex(petsQueryModel.Sexes)
                .FilterByType(petsQueryModel.Types)
                .FilterByColor(petsQueryModel.Colors)
                .Skip(petsQueryModel.Page * petsQueryModel.PageLimit)
                .Take(petsQueryModel.PageLimit);

            return petsFromDB;
        }

        public Pet CreatePet(Pet pet)
        {
            var petWthId =_dbContext.Pets.Add(pet);
            _dbContext.SaveChanges();
            return petWthId.Entity;
        }

        public void UpdatePet(Pet pet)
        {
            var petToUpdate = _dbContext.Pets.SingleOrDefault(p => p.PetId == pet.PetId);
            _dbContext.Entry(petToUpdate).CurrentValues.SetValues(pet);
            _dbContext.SaveChanges();
        }
    }
}
