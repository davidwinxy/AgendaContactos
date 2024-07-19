using System.ComponentModel.DataAnnotations;

namespace Agenda.Models
{
    // Definición de la clase 'LoginViewModel' en el espacio de nombres 'Agenda.Models'
    public class LoginViewModel
    {
        // Propiedad 'Username' que es obligatoria y se muestra con la etiqueta "Username"
        [Required] // Atributo que indica que la propiedad es obligatoria
        [Display(Name = "Username")] // Atributo que define el nombre de la etiqueta a mostrar en la vista
        public string Username { get; set; }

        // Propiedad 'Password' que es obligatoria, se muestra con la etiqueta "Password" y se trata como un campo de contraseña
        [Required] // Atributo que indica que la propiedad es obligatoria
        [DataType(DataType.Password)] // Atributo que define el tipo de datos como contraseña para mostrar un campo de entrada seguro
        [Display(Name = "Password")] // Atributo que define el nombre de la etiqueta a mostrar en la vista
        public string Password { get; set; }
    }
}
