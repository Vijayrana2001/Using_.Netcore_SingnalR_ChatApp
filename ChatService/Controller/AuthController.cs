using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using ChatService.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ChatService.Data;

namespace ChatService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDictionary<string, string> _users;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            // Initialize user dictionary (simulated database)
            _users = new Dictionary<string, string>();
            _context = context;
            _configuration = configuration; 
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // Check if the username is already taken
            var existingUser = await _context.RegisterModels.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (existingUser != null)
            {
                return Conflict("Username is already taken.");
            }
            // Add the user to the database
            var user = new RegisterModel
            {
                Username = model.Username,
                Password = model.Password
            };
            _context.RegisterModels.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Find user by username and password
            var user = await _context.RegisterModels.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
            if (user == null)
            {
                return NotFound("Invalid username or password.");
            }

            // Create token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Appsettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(7), // Token expiry
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
