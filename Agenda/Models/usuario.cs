using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Agenda.Models
{
    public class usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

    }
}
