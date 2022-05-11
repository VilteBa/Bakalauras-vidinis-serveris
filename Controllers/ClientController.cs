using backend.Models;
using backend.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public IConfiguration _configuration;
        public CustomerController(IClientRepository customerService, IConfiguration configuration)
        {
            _clientRepository = customerService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] ClientViewModel clientViewModel)
        {
            var client = new User();
            client.EmailAddress = clientViewModel.EmailAddress;
            client.Role = clientViewModel.Role;
            client.ShelterId = clientViewModel.ShelterId;

            try
            {
                _clientRepository.Register(client, clientViewModel.Password);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] ClientViewModel clientViewModel)
        {
            if (clientViewModel == null) return BadRequest();
            var client = _clientRepository.Authenticate(clientViewModel.EmailAddress, clientViewModel.Password);

            if (client == null) return BadRequest("Invalid credentials");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, client.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                client.UserId,
                client.Role,
                client.EmailAddress,
                client.FirstName,
                client.LastName,
                client.PhoneNumber,
                client.ShelterId,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPatch()]
        public IActionResult UpdateUser(User user)
        {
            _clientRepository.Update(user);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public User Client(Guid id)
        {
            return _clientRepository.GetUser(id);
        }

        // todo: sutvarkyt tuos anonymous
        [AllowAnonymous]
        [HttpGet("{id}/lovedPets")]
        public List<Pet> GetPetsLoved(Guid id, [FromQuery] PetsQueryModel petsQueryModel)
        {
            return _clientRepository.GetUserLovedPets(id, petsQueryModel).ToList();
        }

        // todo: sutvarkyt tuos anonymous
        [AllowAnonymous]
        [HttpGet("{id}/lovedPets/Count")]
        public IActionResult CountPetsLoved(Guid id, [FromQuery] PetsQueryModel petsQueryModel)
        {
            return Ok(_clientRepository.CountUserLovedPets(id, petsQueryModel));
        }

    }
}
