using GestioneSpese.EF.Configuration;
using GestioneSpese.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneSpese.EF
{
    public class GestioneSpeseContext : DbContext
    {
        public GestioneSpeseContext() : base() { }

        public GestioneSpeseContext(DbContextOptions<GestioneSpeseContext> options)
            : base(options) { }

        public DbSet<Spesa> Spese { get; set; }
        public DbSet<Categoria> Categorie { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

                string connectionStringSQL = config.GetConnectionString("GestioneSpese");

                optionsBuilder.UseSqlServer(connectionStringSQL);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SpesaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
        }
    }
}
