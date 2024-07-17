using System.Security.Claims;
using Agenda.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class UsuarioController : Controller
{
    private readonly contextoDb _context;

    public UsuarioController(contextoDb context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var usuario =  _context.usuarios.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (usuario != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim("UsuarioId", usuario.Id.ToString())   

            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                // Configurar propiedades de autenticación si es necesario
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Contactos");
        }

        ViewData["LoginError"] = "Usuario o contraseña incorrectos";
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Usuario");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(usuario usuario)
    {
        if (ModelState.IsValid)
        {
            _context.usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        return View(usuario);
    }
}

