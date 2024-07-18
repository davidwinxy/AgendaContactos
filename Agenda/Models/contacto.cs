using System.ComponentModel.DataAnnotations;

namespace Agenda.Models
{
    public class contacto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(15)]
        public string Telefono { get; set; }

        [Required]
        public string categoria { get; set; }

        public byte[] ImagenPerfil { get; set; }

        [Required]
        public int UsuarioId { get; set; }



        public usuario usuario { get; set; }
    }
}
