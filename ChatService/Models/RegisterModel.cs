using System.ComponentModel.DataAnnotations;

namespace ChatService.Models
{
    public class RegisterModel
    {
        [Key] // Add this attribute to specify the primary key
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}


