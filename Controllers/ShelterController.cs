using backend.Models;
using backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using File = backend.Models.File;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShelterController : ControllerBase
    {
        private readonly IShelterRepository _shelterRepository;
        private readonly IFileRepository _fileRepository;

        public ShelterController(IShelterRepository shelterRepository, IFileRepository fileRepository)
        {
            _shelterRepository = shelterRepository;
            _fileRepository = fileRepository;
        }

        [HttpGet("Cities")]
        public IActionResult GetShelterCities()
        {
            var cities = _shelterRepository.GetShelterCities();
            if (cities == null || !cities.Any())
            {
                return NotFound();
            }
            return Ok(cities);
        }

        [HttpGet]
        public IActionResult GetShelters([FromQuery] SheltersQueryModel sheltersQueryModel)
        {
            var shelters = _shelterRepository.GetShelters(sheltersQueryModel);
            if (shelters == null || !shelters.Any())
            {
                return NotFound();
            }
            return Ok(shelters);
        }

        [HttpGet("Count")]
        public IActionResult CountShelters([FromQuery] SheltersQueryModel sheltersQueryModel)
        {
            return Ok(_shelterRepository.CountShelters(sheltersQueryModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetShelter(Guid id)
        {
            var shelter = _shelterRepository.GetShelter(id);
            if (shelter == null )
            {
                return NotFound();
            }
            return Ok(shelter);
        }

        [HttpPost]
        public IActionResult CreateShelter([FromBody] Shelter shelter)
        {
            _shelterRepository.CreateShelter(shelter);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShelter(Guid id)
        {
            _shelterRepository.DeleteShelter(id);
            return Ok();
        }

        [HttpPatch]
        public IActionResult UpdateShelter(Shelter shelter)
        {
            _shelterRepository.UpdateShelter(shelter);
            return Ok();
        }

        [HttpGet("Pets/{id}")]
        public IActionResult GetShelterPets(Guid id, [FromQuery] PetsQueryModel petsQueryModel)
        {
            var pets = _shelterRepository.GetShelterPets(id, petsQueryModel).ToList();
            if (pets == null || !pets.Any())
            {
                return NotFound();
            }
            return Ok(pets);
        }

        [HttpGet("Pets/{id}/Count")]
        public IActionResult CountShelterPets(Guid id, [FromQuery] PetsQueryModel petsQueryModel)
        {
            return Ok(_shelterRepository.CountShelterPets(id, petsQueryModel));
        }

        [HttpPost("{id}/photo")]
        public IActionResult AddPhotos(Guid id, [FromForm] IFormFile file)
        {
            var data = FileToByteArray(file);

            var currentPhoto = _fileRepository.GetShelterPhoto(id);
            if (currentPhoto != null) _fileRepository.DeleteFile(currentPhoto.FileId);
            _fileRepository.CreateFile(new File { FileName = file.FileName, Data = data, ShelterId = id });

            return Ok();
        }

        [HttpGet("{id}/photo")]
        public IActionResult GetPhotos(Guid id)
        {
            var photo = _fileRepository.GetShelterPhoto(id);
            if (photo == null)
            {
                return NotFound();
            }
            return Ok(photo);
        }

        [HttpDelete("photos/{fileId}")]
        public IActionResult DeletePhoto(Guid fileId)
        {
            _fileRepository.DeleteFile(fileId);
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
