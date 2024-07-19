using Agenda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;


namespace Agenda.Controllers
{
    public class ContactosController : Controller
    {
        private readonly contextoDb contexto;

        // Constructor que inyecta el contexto de la base de datos
        public ContactosController(contextoDb contexto)
        {
            this.contexto = contexto;
        }

        // Acción para mostrar la lista de contactos (GET)
        public async Task<IActionResult> Index(string searchString)
        {
            // Obtener el ID del usuario desde las reclamaciones de la autenticación
            var userIdClaim = User.FindFirstValue("UsuarioId");
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            // Consultar los contactos del usuario
            var contactosQuery = contexto.Contactos
                .Where(c => c.UsuarioId == userId);

            // Aplicar filtro de búsqueda si se proporciona un término de búsqueda
            if (!string.IsNullOrEmpty(searchString))
            {
                contactosQuery = contactosQuery.Where(s => s.Nombre.Contains(searchString));
            }

            // Obtener la lista de contactos ordenada por ID en orden descendente
            var contactos = await contactosQuery
                .OrderByDescending(d => d.Id)
                .ToListAsync();

            return View(contactos);
        }

        // Acción para mostrar los detalles de un contacto específico (GET)
        public async Task<IActionResult> Details(int id)
        {
            var contacto = await contexto.Contactos.SingleOrDefaultAsync(d => d.Id == id);
            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // Acción para mostrar la vista de creación de un nuevo contacto (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar la creación de un nuevo contacto (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(contacto contacto)
        {
            if (ModelState.IsValid)
            {
                // Asignar automáticamente el ID del usuario actual al nuevo contacto
                var userIdClaim = User.FindFirstValue("UsuarioId");
                if (int.TryParse(userIdClaim, out int userId))
                {
                    contacto.UsuarioId = userId;

                    // Generar una imagen de perfil para el contacto
                    contacto.ImagenPerfil = GenerarImagenPerfil(contacto.Nombre);

                    // Agregar el nuevo contacto a la base de datos y guardar los cambios
                    contexto.Contactos.Add(contacto);
                    await contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid user ID.");
                    return View(contacto);
                }
            }

            return View(contacto);
        }

        // Método para generar una imagen de perfil para un contacto
        private byte[] GenerarImagenPerfil(string nombre)
        {
            int width = 1000;
            int height = 1000;
            string letra = nombre.Substring(0, 1).ToUpper();

            var font = SystemFonts.CreateFont("Times New Roman", 500, FontStyle.Bold);
            Random rnd = new Random();
            var colorAleatorio = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));

            using (var image = new Image<Rgba32>(width, height))
            {
                image.Mutate(ctx =>
                {
                    ctx.Fill(colorAleatorio);
                    ctx.DrawText(letra, font, Color.White, new PointF(300, 250));
                });

                using (var ms = new MemoryStream())
                {
                    image.SaveAsPng(ms);
                    return ms.ToArray();
                }
            }
        }

        // Acción para mostrar la vista de edición de un contacto específico (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var contacto = await contexto.Contactos.FindAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // Acción para manejar la edición de un contacto específico (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, contacto contacto, byte[] ImagenPerfil)
        {
            var userIdClaim = User.FindFirstValue("UsuarioId");

            if (id != contacto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contacto.ImagenPerfil = ImagenPerfil;
                    contexto.Update(contacto);
                    await contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!contexto.Contactos.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(contacto);
        }

        // Acción para mostrar la vista de eliminación de un contacto específico (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirstValue("UsuarioId");
            var contacto = await contexto.Contactos
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == Convert.ToInt32(userIdClaim));

            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // Acción para manejar la eliminación de un contacto específico (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdClaim = User.FindFirstValue("UsuarioId");
            var contacto = await contexto.Contactos
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == Convert.ToInt32(userIdClaim));

            if (contacto == null)
            {
                return NotFound();
            }

            contexto.Contactos.Remove(contacto);
            await contexto.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}