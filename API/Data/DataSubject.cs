using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataSubject : DbContext
    {
        public DataSubject(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}