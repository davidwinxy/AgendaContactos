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
        public ContactosController(contextoDb contexto)
        {
            this.contexto = contexto;
        }
        // GET: ContactosController
        public async Task<IActionResult> Index(string searchString)
        {
            var userIdClaim = User.FindFirstValue("UsuarioId");
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var contactosQuery = contexto.Contactos
                .Where(c => c.UsuarioId == userId);

            if (!String.IsNullOrEmpty(searchString))
            {
                contactosQuery = contactosQuery.Where(s => s.Nombre.Contains(searchString));
            }

            var contactos = await contactosQuery
                .OrderByDescending(d => d.Id)
                .ToListAsync();

            return View(contactos);
        }
        // GET: ContactosController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contactos = await contexto.Contactos.SingleOrDefaultAsync(d => d.Id == id);

            return View(contactos);
        }

        // GET: ContactosController/Create
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(contacto contactos)
        {
           
                // Asignar automáticamente el Id del usuario actual
                var userIdClaim = User.FindFirstValue("UsuarioId");
                if (int.TryParse(userIdClaim, out int userId))
                {
                    contactos.UsuarioId = userId;

                    contactos.ImagenPerfil = GenerarImagenPerfil(contactos.Nombre);


                    contexto.Contactos.Add(contactos);
                    await contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid user ID.");
                    return View(contactos);
                }
        }



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




        // GET: ContactosController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contactos = await contexto.Contactos.FindAsync(id);
            if (contactos == null)
            {
                return NotFound();
            }
            return View(contactos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, contacto contactos, byte[] ImagenPerfil)
        {
            var userIdClaim = User.FindFirstValue("UsuarioId");
          
   
                if (id != contactos.Id)
            {
                return BadRequest();
            }

           
                try
                {
                    contactos.ImagenPerfil = ImagenPerfil;
                    contexto.Update(contactos);
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

        // GET: ContactosController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirstValue("UsuarioId");
            var contactos = await contexto.Contactos
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == Convert.ToInt32(userIdClaim));

            if (contactos == null)
            {
                return NotFound();
            }

            return View(contactos);
        }

        // POST: ContactosController/Delete/5
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
        /* public IActionResult Delete(int id)
         {
             return View();
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Delete(int id, contacto contacto)
         {
             var contactos = await contexto.Contactos.FindAsync(id);
             contexto.Contactos.Remove(contacto);
             await contexto.SaveChangesAsync();

             return RedirectToAction("Index");
         }*/
    }
}
