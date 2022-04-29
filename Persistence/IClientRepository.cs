using backend.Controllers;
using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.Persistence
{
    public interface IClientRepository
    {
        User Authenticate(string emailAdress, string password);
        User Register(User client, string password);
        User GetUser(Guid userId);
        void Update(User user);
        IEnumerable<Pet> GetUserLovedPets(Guid id, PetsQueryModel petsQueryModel);
        int CountUserLovedPets(Guid id, PetsQueryModel petsQueryModel);
    }
}
