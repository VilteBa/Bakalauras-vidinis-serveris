using backend.Controllers;
using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.Persistence
{
    public interface IPetRepository
    {
        IEnumerable<Pet> GetPets(PetsQueryModel petsQueryModel);
        Pet GetPet(Guid petId);
        Pet CreatePet(Pet pet);
        void DeletePet(Guid id);
        void UpdatePet(Pet pet);
    }
}