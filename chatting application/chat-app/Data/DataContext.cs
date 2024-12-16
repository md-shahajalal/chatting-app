using chat_app.Entities;
using Microsoft.EntityFrameworkCore;

namespace chat_app.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
}
