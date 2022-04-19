using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Persistence
{
    public interface IClientRepository
    {
        User Authenticate(string emailAdress, string password);
        User Register(User client, string password);
        User GetUser(Guid userId);
    }
}
