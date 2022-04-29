using backend.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using backend.Controllers;

namespace backend.Persistence
{
    public class ClientRepository : IClientRepository
    {
        private readonly DBContext _dbContext;

        public ClientRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User Authenticate(string emailAdress, string password)
        {
            if (string.IsNullOrEmpty(emailAdress) || (string.IsNullOrEmpty(password)))
            {
                return null;
            }

            var client = _dbContext.Users.SingleOrDefault(x => x.EmailAddress == emailAdress);

            if (client == null)
            {
                return null;
            }

            return !VerifyPasswordHash(password, client.PasswordHash, client.PasswordSalt) ? null : client;
        }

        public User Register(User client, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ApplicationException("Slaptažodis privalomas!");
            }

            if (!IsValidEmail(client.EmailAddress))
            {
                throw new ApplicationException("Įveskite tinkamą el. paštą!");
            }

            if (_dbContext.Users.Any(x => x.EmailAddress == client.EmailAddress))
            {
                throw new ApplicationException("Vartotojo el. paštas " + client.EmailAddress + " jau užimtas");
            }

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            client.PasswordHash = passwordHash;
            client.PasswordSalt = passwordSalt;

            _dbContext.Users.Add(client);
            _dbContext.SaveChanges();

            return client;
        }

        //public IEnumerable<Pet> GetLovedPets(Guid clientId)
        //{
        //    var client = _dbContext.Clients.SingleOrDefault(client => client.ClientId == clientId);
        //    return client.LovedPets;
        //}

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(nameof(password));

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(nameof(password));

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != storedHash[i]).Any();
        }

        static bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public User GetUser(Guid userId)
        {
            return _dbContext.Users.Include(u => u.Pets).SingleOrDefault(user => user.UserId == userId);
        }

        public void Update(User user)
        {
            var userToUpdate = _dbContext.Users.SingleOrDefault(p => p.UserId == user.UserId);
            _dbContext.Entry(userToUpdate).CurrentValues.SetValues(user);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Pet> GetUserLovedPets(Guid id, PetsQueryModel petsQueryModel)
        {
            var lovedPets = GetUser(id).LovedPets.Select(lp => lp.Pet);
            return lovedPets
                .FilterByCity(petsQueryModel.Cities)
                .FilterBySize(petsQueryModel.Sizes)
                .FilterBySex(petsQueryModel.Sexes)
                .FilterByType(petsQueryModel.Types)
                .FilterByColor(petsQueryModel.Colors)
                .FilterByAge(petsQueryModel.MinAge, petsQueryModel.MaxAge)
                .Skip(petsQueryModel.Page * petsQueryModel.PageLimit)
                .Take(petsQueryModel.PageLimit);

        }

        public int CountUserLovedPets(Guid id, PetsQueryModel petsQueryModel)
        {
            var lovedPets = GetUser(id).LovedPets.Select(lp => lp.Pet);
            return lovedPets
                .FilterByCity(petsQueryModel.Cities)
                .FilterBySize(petsQueryModel.Sizes)
                .FilterBySex(petsQueryModel.Sexes)
                .FilterByType(petsQueryModel.Types)
                .FilterByColor(petsQueryModel.Colors)
                .FilterByAge(petsQueryModel.MinAge, petsQueryModel.MaxAge)
                .Count();

        }
    }
}