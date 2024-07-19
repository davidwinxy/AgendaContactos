using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Agenda.Models
{
    // Definición de la clase 'contextoDb' que hereda de DbContext
    public class contextoDb : DbContext
    {
        // Constructor que recibe opciones de configuración para el DbContext
        public contextoDb(DbContextOptions<contextoDb> opciones) : base(opciones)
        {
        }

        // Propiedad que representa la colección de usuarios en la base de datos
        public DbSet<usuario> usuarios { get; set; }

        // Propiedad que representa la colección de contactos en la base de datos
        public DbSet<contacto> Contactos { get; set; }

        // Método protegido para configurar el modelo de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llamada al método base para aplicar configuraciones predeterminadas
            base.OnModelCreating(modelBuilder);

            // Aquí se pueden realizar configuraciones adicionales del modelo
            // Por ejemplo, establecer restricciones de claves foráneas, índices únicos, etc.
        }
    }
}

