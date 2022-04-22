using backend.Controllers;
using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.Persistence
{
    public interface IShelterRepository
    {
        IEnumerable<Shelter> GetShelters(SheltersQueryModel sheltersQueryModel);
        Shelter GetShelter(Guid id);
        void UpdateShelter(Shelter shelter);
        void CreateShelter(Shelter shelter);
        void DeleteShelter(Guid id);
    }
}