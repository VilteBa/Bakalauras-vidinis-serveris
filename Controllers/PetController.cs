using backend.Models;
using backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly DBContext _dbContext;
        private readonly IPetRepository _petRepository;

        public PetController(IPetRepository testPetRepo, DBContext dbContext)
        {
            _petRepository = testPetRepo;
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
            return _petRepository.GetPets(petsQueryModel);
        }


        [HttpGet("Count")]
        public IActionResult CountPets([FromQuery] PetsQueryModel petsQueryModel)
        {
            return Ok(_petRepository.CountPets(petsQueryModel));
        }

        [HttpGet("{id}")]
        public Pet GetPet(Guid id)
        {
            return _petRepository.GetPet(id);
        }

        [HttpGet("{id}/shelter")]
        public Shelter GetPetShelter(Guid id)
        {
            return _petRepository.GetPet(id).Shelter;
        }

        [HttpPost]
        public IActionResult CreatePet([FromBody] Pet pet)
        {
            var createdPet = _petRepository.CreatePet(pet);
            return Ok(createdPet);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePet(Guid id)
        {
            _petRepository.DeletePet(id);
            return Ok();
        }

        [HttpPatch]
        public IActionResult UpdatePet(Pet pet)
        {
            _petRepository.UpdatePet(pet);
            return Ok();
        }

        // todo: perktelt prie user
        [HttpGet("{id}/loved")]
        public List<User> GetPetsLoved(Guid id)
        {
            return _petRepository.GetPet(id).LovedPets.Select(lp => lp.User).ToList();
        }

        [HttpPut("LovePet")]
        public Pet AddLovedPet(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.Include(u => u.LovedPets).SingleOrDefault(u => u.UserId == userId);
            var petToAdd = _petRepository.GetPet(petId);
            user.LovedPets.Add(new LovedPets() { User = user, Pet = petToAdd});
            _dbContext.SaveChanges();
            return _petRepository.GetPet(petId);
        }

        [HttpDelete("UnlovePet")]
        public IActionResult DeleteLovedPet(Guid petId, Guid userId)
        {
            var user = _dbContext.Users.Include(u => u.LovedPets).SingleOrDefault(u => u.UserId == userId);
            var lovedPet = user.LovedPets.SingleOrDefault(x => x.UserId == userId && x.PetId==petId);
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
            var pet = _petRepository.GetPet(petId);
            return user.ShelterId==pet.ShelterId;
        }

        //[HttpPost]
        //public IActionResult Post()
        //{
        //    //var postedFile = HttpContext.Current.Request.Files.AllKeys.Any()
        //    //byte[] bytes;
        //    //using (BinaryReader br = new BinaryReader(postedFile.InputStream))
        //    //{
        //    //    bytes = br.ReadBytes(postedFile.ContentLength);
        //    //}
        //    //return Ok();
        //}
    }
}
