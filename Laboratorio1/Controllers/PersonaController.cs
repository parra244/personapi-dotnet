using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    // ── MVC Controller (Views) ───────────────────────────────────────────────
    public class PersonaController : Controller
    {
        private readonly IPersonaRepository _repo;

        public PersonaController(IPersonaRepository repo)
        {
            _repo = repo;
        }

        // GET: /Persona
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        // GET: /Persona/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        // GET: /Persona/Create
        public IActionResult Create() => View();

        // POST: /Persona/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Persona persona)
        {
            if (!ModelState.IsValid) return View(persona);
            await _repo.CreateAsync(persona);
            TempData["Success"] = "Persona creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Persona/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        // POST: /Persona/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Persona persona)
        {
            if (id != persona.Cc) return BadRequest();
            if (!ModelState.IsValid) return View(persona);

            var result = await _repo.UpdateAsync(id, persona);
            if (result == null) return NotFound();

            TempData["Success"] = "Persona actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Persona/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var persona = await _repo.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return View(persona);
        }

        // POST: /Persona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _repo.DeleteAsync(id);
            TempData["Success"] = "Persona eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }

    // ── API Controller (REST / Swagger) ──────────────────────────────────────
    [ApiController]
    [Route("api/Persona")]
    public class PersonaApiController : ControllerBase
    {
        private readonly IPersonaRepository _repo;

        public PersonaApiController(IPersonaRepository repo)
        {
            _repo = repo;
        }

        /// <summary>Obtiene todas las personas.</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Persona>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        /// <summary>Obtiene una persona por su cédula.</summary>
        [HttpGet("{cc}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Persona>> GetById(long cc)
        {
            var persona = await _repo.GetByIdAsync(cc);
            return persona == null ? NotFound() : Ok(persona);
        }

        /// <summary>Crea una nueva persona.</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Persona>> Create(Persona persona)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _repo.ExistsAsync(persona.Cc))
                return BadRequest("Ya existe una persona con esa cédula.");

            var created = await _repo.CreateAsync(persona);
            return CreatedAtAction(nameof(GetById), new { cc = created.Cc }, created);
        }

        /// <summary>Actualiza una persona existente.</summary>
        [HttpPut("{cc}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Persona>> Update(long cc, Persona persona)
        {
            if (cc != persona.Cc) return BadRequest("La cédula no coincide.");
            var updated = await _repo.UpdateAsync(cc, persona);
            return updated == null ? NotFound() : Ok(updated);
        }

        /// <summary>Elimina una persona.</summary>
        [HttpDelete("{cc}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long cc)
        {
            var deleted = await _repo.DeleteAsync(cc);
            return deleted ? NoContent() : NotFound();
        }
    }
}
