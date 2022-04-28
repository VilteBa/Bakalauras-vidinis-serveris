using System;

namespace backend.Models
{
    public class ClientViewModel
    {
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public Guid? ShelterId { get; set; }
    }
}
