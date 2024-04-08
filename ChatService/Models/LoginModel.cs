using System.ComponentModel.DataAnnotations;

namespace ChatService.Models
{
    public class LoginModel
    {
        [Key] // Add this attribute to specify the primary key
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
