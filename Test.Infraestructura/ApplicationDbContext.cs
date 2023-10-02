using Microsoft.EntityFrameworkCore;
using Test.Dominio.Entities;

namespace Test.Infraestructura
{

    public class ApplicationDbContext : DbContext
    {

        //private readonly string _connectionString;

        //public ApplicationDbContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_connectionString);
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
    }


}
