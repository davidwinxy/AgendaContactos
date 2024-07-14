using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Agenda.Models
{
    public class contextoDb : DbContext
    {
        public contextoDb(DbContextOptions opciones) : base(opciones)
        {
        }

        public DbSet<usuario> usuarios { get; set; }
        public DbSet<contacto> Contactos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }


    }
}
