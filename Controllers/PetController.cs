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
        public IActionResult GetSizes()
        {
            return Ok(Enum.GetValues(typeof(Size)));
        }

        [HttpGet("sexes")]
        public IActionResult GetSexes()
        {
            return Ok(Enum.GetValues(typeof(Sex)));
        }

        [HttpGet("colors")]
        public IActionResult GetColors()
        {
            return Ok(Enum.GetValues(typeof(Color)));
        }

        [HttpGet("types")]
        public IActionResult GetTypes()
        {
            return Ok(Enum.GetValues(typeof(PetType)));
        }

        [HttpGet]
        public IActionResult GetPets([FromQuery] PetsQueryModel petsQueryModel)
        {
            var pets = _petRepo.GetPets(petsQueryModel);
            if (pets == null || !pets.Any())
            {
                return NotFound();
            }
            return Ok(pets);
        }


        [HttpGet("Count")]
        public IActionResult CountPets([FromQuery] PetsQueryModel petsQueryModel)
        {
            return Ok(_petRepo.CountPets(petsQueryModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetPet(Guid id)
        {
            var pet = _petRepo.GetPet(id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }

        [HttpGet("{id}/shelter")]
        public IActionResult GetPetShelter(Guid id)
        {
            var pet = _petRepo.GetPet(id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet.Shelter);
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

        [HttpPut("LovePet")]
        public IActionResult AddLovedPet(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.Include(u => u.LovedPets).SingleOrDefault(u => u.UserId == userId);
            var petToAdd = _petRepo.GetPet(petId);
            user.LovedPets.Add(new LovedPets() { User = user, Pet = petToAdd });
            _dbContext.SaveChanges();
            return Ok(_petRepo.GetPet(petId));
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
        public IActionResult IsEditableByUser(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.UserId == userId);
            var pet = _petRepo.GetPet(petId);
            return Ok(user.ShelterId == pet.ShelterId);
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
            var photos = _fileRepo.GetPetPhotos(id);
            if (photos == null || !photos.Any())
            {
                return NotFound();
            }
            return Ok(photos);
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
