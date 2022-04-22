﻿using backend.Models;
using backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShelterController : ControllerBase
    {
        private readonly IShelterRepository _shelterRepository;
        public ShelterController(IShelterRepository shelterRepository)
        {
            _shelterRepository = shelterRepository;
        }

        [HttpGet]
        public IEnumerable<Shelter> GetShelters([FromQuery] SheltersQueryModel sheltersQueryModel)
        {
            return _shelterRepository.GetShelters(sheltersQueryModel);
        }

        [HttpGet("{id}")]
        public Shelter GetShelter(Guid id)
        {
            return _shelterRepository.GetShelter(id);
        }

        [HttpGet("Pets/{id}")]
        public List<Pet> GetShelterPets(Guid id)
        {
            return _shelterRepository.GetShelter(id).Pets.ToList();
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

    }
}
