using backend.Models;
using backend.Persistence;
using backend.RequestModels;
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
        public IEnumerable<string> GetShelterCities()
        {
            return _shelterRepository.GetShelterCities();
        }

        [HttpGet]
        public IEnumerable<Shelter> GetShelters([FromQuery] SheltersQueryModel sheltersQueryModel)
        {
            return _shelterRepository.GetShelters(sheltersQueryModel);
        }

        [HttpGet("Count")]
        public IActionResult CountShelters([FromQuery] SheltersQueryModel sheltersQueryModel)
        {
            return Ok(_shelterRepository.CountShelters(sheltersQueryModel));
        }

        [HttpGet("{id}")]
        public Shelter GetShelter(Guid id)
        {
            return _shelterRepository.GetShelter(id);
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
        public List<Pet> GetShelterPets(Guid id, [FromQuery] PetsQueryModel petsQueryModel)
        {
            return _shelterRepository.GetShelterPets(id, petsQueryModel).ToList();
        }

        [HttpGet("Pets/{id}/Count")]
        public IActionResult CountShelterPets(Guid id, [FromQuery] PetsQueryModel petsQueryModel)
        {
            return Ok(_shelterRepository.CountShelterPets(id, petsQueryModel));
        }

        [HttpPost("{id}/photo")]
        public IActionResult AddPhotos(Guid id, [FromForm] FileModel photo)
        {
            var data = FileToByteArray(photo);

            var currentPhoto = _fileRepository.GetShelterPhoto(id);
            if (currentPhoto != null) _fileRepository.DeleteFile(currentPhoto.FileId);
            _fileRepository.CreateFile(new File { FileName = photo.FileName, Data = data, ShelterId = id });

            return Ok();
        }

        [HttpGet("{id}/photo")]
        public IActionResult GetPhotos(Guid id)
        {
            return Ok(_fileRepository.GetShelterPhoto(id));
        }

        [HttpDelete("photos/{fileId}")]
        public IActionResult DeletePhoto(Guid fileId)
        {
            _fileRepository.DeleteFile(fileId);
            return Ok();
        }

        private static byte[] FileToByteArray(FileModel file)
        {
            using (var ms = new MemoryStream())
            {
                file.FormFile.CopyTo(ms);
                return ms.ToArray();
            }
        }
     }
}
