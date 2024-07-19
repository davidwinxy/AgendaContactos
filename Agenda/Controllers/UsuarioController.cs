using System.Security.Claims;
using Agenda.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Controllers
{
    public class UsuarioController : Controller
    {
        // Campo privado para el contexto de la base de datos
        private readonly contextoDb _context;

        // Constructor del controlador que inyecta el contexto de la base de datos
        public UsuarioController(contextoDb context)
        {
            _context = context;
        }

        // Acción para mostrar la vista de inicio de sesión (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Acción para manejar el inicio de sesión (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Buscar el usuario en la base de datos que coincide con el nombre de usuario y contraseña proporcionados
            var usuario = _context.usuarios.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (usuario != null)
            {
                // Crear una lista de reclamaciones (claims) para la autenticación
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Username), // Reclamación para el nombre de usuario
                    new Claim("UsuarioId", usuario.Id.ToString())   // Reclamación para el ID del usuario
                };

                // Crear un objeto ClaimsIdentity con las reclamaciones y el esquema de autenticación de cookies
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Configurar las propiedades de autenticación (opcional)
                var authProperties = new AuthenticationProperties
                {
                    // Configurar propiedades de autenticación si es necesario
                };

                // Iniciar sesión con las reclamaciones y propiedades de autenticación
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirigir al usuario a la acción "Index" del controlador "Contactos"
                return RedirectToAction("Index", "Contactos");
            }

            // Si el usuario no se encuentra, mostrar un mensaje de error
            ViewData["LoginError"] = "Usuario o contraseña incorrectos";
            return View();
        }

        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión y eliminar las cookies de autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirigir a la acción "Login"
            return RedirectToAction(nameof(Login));
        }

        // Acción para mostrar la vista de registro (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Acción para manejar el registro de nuevos usuarios (POST)
        [HttpPost]
        public async Task<IActionResult> Register(usuario usuario)
        {
            // Verificar si el modelo es válido (todos los campos requeridos están completos)
            if (ModelState.IsValid)
            {
                // Agregar el nuevo usuario a la base de datos
                _context.usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Redirigir al usuario a la acción "Login"
                return RedirectToAction("Login");
            }

            // Si el modelo no es válido, mostrar la vista de registro con los errores
            return View(usuario);
        }
    }
}

