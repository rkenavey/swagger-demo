using System;
using System.Collections.Generic;

namespace SwaggerDemo.Models
{
    public class Player
    {
        public int? Id { get; set; }
        public int TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Joined { get; set; }
        public bool IsCaptain { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> PhoneNumbers { get; set; }
        public Address Address { get; set; }
        public DateTime? Dob { get; set; }
    }
}
