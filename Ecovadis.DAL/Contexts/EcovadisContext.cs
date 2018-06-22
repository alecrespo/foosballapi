using Ecovadis.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ecovadis.DAL.Contexts
{
    public class EcovadisContext : DbContext
    {
        public EcovadisContext(DbContextOptions<EcovadisContext> options) : base(options)
        {
        }
        public DbSet<Goal> Goal { get; set; }
        public DbSet<Period> Period { get; set; }
        public DbSet<Match> Match { get; set; }
    }
    public class ApplicationContextDbFactory : IDesignTimeDbContextFactory<EcovadisContext>
    {
        EcovadisContext IDesignTimeDbContextFactory<EcovadisContext>.CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            var sqlConnection = root.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<EcovadisContext>();
            optionsBuilder.UseSqlServer<EcovadisContext>(sqlConnection);

            return new EcovadisContext(optionsBuilder.Options);
        }
    }
}
