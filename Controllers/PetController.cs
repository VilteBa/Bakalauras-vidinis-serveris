using backend.Models;
using backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using File = backend.Models.File;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly DBContext _dbContext;
        private readonly IPetRepository _petRepo;
        private readonly IFileRepository _fileRepo;

        public PetController(IPetRepository petRepo, IFileRepository fileRepo, DBContext dbContext)
        {
            _petRepo = petRepo;
            _fileRepo = fileRepo;
            _dbContext = dbContext;
        }

        [HttpGet("sizes")]
        public Array GetSizes()
        {
            return Enum.GetValues(typeof(Size));
        }

        [HttpGet("sexes")]
        public Array GetSexes()
        {
            return Enum.GetValues(typeof(Sex));
        }

        [HttpGet("colors")]
        public Array GetColors()
        {
            return Enum.GetValues(typeof(Color));
        }

        [HttpGet("types")]
        public Array GetTypes()
        {
            return Enum.GetValues(typeof(PetType));
        }

        [HttpGet]
        public IEnumerable<Pet> GetPets([FromQuery] PetsQueryModel petsQueryModel)
        {
            return _petRepo.GetPets(petsQueryModel);
        }


        [HttpGet("Count")]
        public IActionResult CountPets([FromQuery] PetsQueryModel petsQueryModel)
        {
            return Ok(_petRepo.CountPets(petsQueryModel));
        }

        [HttpGet("{id}")]
        public Pet GetPet(Guid id)
        {
            return _petRepo.GetPet(id);
        }

        [HttpGet("{id}/shelter")]
        public Shelter GetPetShelter(Guid id)
        {
            return _petRepo.GetPet(id).Shelter;
        }

        [HttpPost]
        public IActionResult CreatePet([FromBody] Pet pet)
        {
            var createdPet = _petRepo.CreatePet(pet);
            return Ok(createdPet);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePet(Guid id)
        {
            _petRepo.DeletePet(id);
            return Ok();
        }

        [HttpPatch]
        public IActionResult UpdatePet(Pet pet)
        {
            _petRepo.UpdatePet(pet);
            return Ok();
        }

        [HttpGet("{id}/loved")]
        public List<User> GetPetsLoved(Guid id)
        {
            return _petRepo.GetPet(id).LovedPets.Select(lp => lp.User).ToList();
        }

        [HttpPut("LovePet")]
        public Pet AddLovedPet(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.Include(u => u.LovedPets).SingleOrDefault(u => u.UserId == userId);
            var petToAdd = _petRepo.GetPet(petId);
            user.LovedPets.Add(new LovedPets() { User = user, Pet = petToAdd });
            _dbContext.SaveChanges();
            return _petRepo.GetPet(petId);
        }

        [HttpDelete("UnlovePet")]
        public IActionResult DeleteLovedPet(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.Include(u => u.LovedPets).SingleOrDefault(u => u.UserId == userId);
            var lovedPet = user.LovedPets.SingleOrDefault(x => x.UserId == userId && x.PetId == petId);
            user.LovedPets.Remove(lovedPet);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("isLovedPet")]
        public IActionResult IsLovedPet(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.Include(u => u.LovedPets).SingleOrDefault(u => u.UserId == userId);
            var isLovedPet = user.LovedPets.Any(x => x.UserId == userId && x.PetId == petId);
            return Ok(isLovedPet);
        }


        [HttpGet("Editable")]
        public Boolean IsEditableByUser(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.UserId == userId);
            var pet = _petRepo.GetPet(petId);
            return user.ShelterId == pet.ShelterId;
        }

        [HttpPost("{id}/photos")]
        public IActionResult AddPhotos(Guid id, [FromForm] List<IFormFile> files)
        {
            foreach (var photo in files)
            {
                var data = FileToByteArray(photo);

                _fileRepo.CreateFile(new File { FileName = photo.FileName, Data = data, PetId = id });
            }

            return Ok();
        }

        [HttpGet("{id}/photos")]
        public IActionResult GetPhotos(Guid id)
        {
            return Ok(_fileRepo.GetPetPhotos(id));
        }

        [HttpDelete("photos/{fileId}")]
        public IActionResult DeletePhoto(Guid fileId)
        {
            _fileRepo.DeleteFile(fileId);
            return Ok();
        }

        private static byte[] FileToByteArray(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
