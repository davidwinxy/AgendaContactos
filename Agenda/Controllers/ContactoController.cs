using Agenda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

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
        public async Task<IActionResult> Index(int? page)
        {
            var userIdClaim = User.FindFirstValue("UsuarioId");


            int pageSize = 10;
            int pageNumber = page ?? 1;

            var contactos = await contexto.Contactos
                .Where(c => c.UsuarioId == Convert.ToInt32(userIdClaim))
                .OrderByDescending(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
            
            
        

        /*
          [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(contacto contactos)
    {
        if (ModelState.IsValid)
        {
            // Obtener automáticamente el Id del usuario actual
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int userId))
            {
                contactos.UsuarioId = userId;

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
        else
        {
            return View(contactos);
        }
    }
        */
         

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
        public async Task<IActionResult> Edit(int id, contacto contactos)
        {
            if (id != contactos.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            return View(contactos);
        }

        // GET: ContactosController/Delete/5
        public IActionResult Delete(int id)
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
        }
    }
}
