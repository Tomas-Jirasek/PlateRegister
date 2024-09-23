using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DbContexts
{
    public class PlatesDbContext : DbContext
    {
        public DbSet<Plate> Plates { get; set; } = null;

        public PlatesDbContext(DbContextOptions<PlatesDbContext> options) : base(options)
        {

        }
    }
}
