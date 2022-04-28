using backend.Models;
using System;

namespace backend.Persistence
{
    public interface IClientRepository
    {
        User Authenticate(string emailAdress, string password);
        User Register(User client, string password);
        User GetUser(Guid userId);
        void Update(User user);
    }
}
