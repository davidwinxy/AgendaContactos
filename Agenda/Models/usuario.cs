using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Agenda.Models
{
    // Definición de la clase 'usuario' en el espacio de nombres 'Agenda.Models'
    public class usuario
    {
        // Propiedad 'Id' que sirve como clave primaria para la entidad 'usuario'
        [Key]
        public int Id { get; set; }

        // Propiedad 'Username' que es obligatoria y tiene una longitud máxima de 100 caracteres
        [Required] // Atributo que indica que la propiedad es obligatoria
        [MaxLength(100)] // Atributo que define la longitud máxima permitida para esta propiedad
        public string Username { get; set; }

        // Propiedad 'Password' que es obligatoria y tiene una longitud máxima de 100 caracteres
        [Required] // Atributo que indica que la propiedad es obligatoria
        [MaxLength(100)] // Atributo que define la longitud máxima permitida para esta propiedad
        public string Password { get; set; }
    }
}

