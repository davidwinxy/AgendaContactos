using System.ComponentModel.DataAnnotations;

namespace Agenda.Models
{
    // Definición de la clase 'contacto' que representa una entidad en la base de datos
    public class contacto
    {
        // Propiedad que actúa como la clave primaria para la entidad 'contacto'
        [Key]
        public int Id { get; set; }

        // Propiedad para almacenar el nombre del contacto, obligatoria y con un límite de 100 caracteres
        [Required] // Atributo que indica que el campo es obligatorio
        [MaxLength(100)] // Atributo que define el límite máximo de caracteres permitidos
        public string Nombre { get; set; }

        // Propiedad para almacenar el número de teléfono del contacto, obligatoria y con un límite de 15 caracteres
        [Required] // Atributo que indica que el campo es obligatorio
        [MaxLength(15)] // Atributo que define el límite máximo de caracteres permitidos
        public string Telefono { get; set; }

        // Propiedad para almacenar la categoría del contacto, obligatoria
        [Required] // Atributo que indica que el campo es obligatorio
        public string categoria { get; set; }

        // Propiedad opcional para almacenar una imagen de perfil del contacto en formato binario
        public byte[] ImagenPerfil { get; set; }

        // Propiedad para almacenar la clave foránea que relaciona el contacto con un usuario, obligatoria
        [Required] // Atributo que indica que el campo es obligatorio
        public int UsuarioId { get; set; }

        // Navegación a la entidad 'usuario' que posee el contacto
        public usuario usuario { get; set; }
    }
}

