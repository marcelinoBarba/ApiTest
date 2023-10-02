using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Dominio.Entities;

namespace Test.Infraestructura
{
    public class TenantDbContext : DbContext
    {
        private readonly string _connectionString;


        public TenantDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "";
            if (_connectionString == null)
            {
                connectionString = "Server=(localdb)\\mssqllocaldb;Database=Tenant1DB;Trusted_Connection=True;MultipleActiveResultSets=true;";
            } else
            {
                connectionString = _connectionString;
            }

            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Product> Products { get; set; }
    }
}
