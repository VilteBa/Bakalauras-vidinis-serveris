using backend.Controllers;
using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.Persistence
{
    public interface IShelterRepository
    {
        IEnumerable<Shelter> GetShelters(SheltersQueryModel sheltersQueryModel);
        IEnumerable<string> GetShelterCities();
        Shelter GetShelter(Guid id);
        void UpdateShelter(Shelter shelter);
        void CreateShelter(Shelter shelter);
        void DeleteShelter(Guid id);
        int CountShelters(SheltersQueryModel sheltersQueryModel);
        IEnumerable<Pet> GetShelterPets(Guid id, PetsQueryModel petsQueryModel);
        int CountShelterPets(Guid id, PetsQueryModel petsQueryModel);
    }
}