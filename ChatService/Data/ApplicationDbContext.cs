using ChatService.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<RegisterModel>RegisterModels { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }

    }
}
