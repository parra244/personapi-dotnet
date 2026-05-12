using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    // ── MVC Controller ───────────────────────────────────────────────────────
    public class TelefonoController : Controller
    {
        private readonly ITelefonoRepository _repo;
        private readonly IPersonaRepository  _personaRepo;

        public TelefonoController(ITelefonoRepository repo, IPersonaRepository personaRepo)
        {
            _repo        = repo;
            _personaRepo = personaRepo;
        }

        public async Task<IActionResult> Index()
            => View(await _repo.GetAllAsync());

        public async Task<IActionResult> Details(string id)
        {
            var telefono = await _repo.GetByIdAsync(id);
            return telefono == null ? NotFound() : View(telefono);
        }

        public async Task<IActionResult> Create()
        {
            await LoadPersonasAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Telefono telefono)
        {
            if (!ModelState.IsValid)
            {
                await LoadPersonasAsync();
                return View(telefono);
            }
            await _repo.CreateAsync(telefono);
            TempData["Success"] = "Teléfono registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var telefono = await _repo.GetByIdAsync(id);
            if (telefono == null) return NotFound();
            await LoadPersonasAsync();
            return View(telefono);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Telefono telefono)
        {
            if (!ModelState.IsValid)
            {
                await LoadPersonasAsync();
                return View(telefono);
            }
            var result = await _repo.UpdateAsync(id, telefono);
            if (result == null) return NotFound();
            TempData["Success"] = "Teléfono actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var telefono = await _repo.GetByIdAsync(id);
            return telefono == null ? NotFound() : View(telefono);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _repo.DeleteAsync(id);
            TempData["Success"] = "Teléfono eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadPersonasAsync()
        {
            var personas = await _personaRepo.GetAllAsync();
            ViewBag.Personas = new SelectList(personas, "Cc", "Nombre");
        }
    }

    // ── API Controller ───────────────────────────────────────────────────────
    [ApiController]
    [Route("api/Telefono")]
    public class TelefonoApiController : ControllerBase
    {
        private readonly ITelefonoRepository _repo;

        public TelefonoApiController(ITelefonoRepository repo)
        {
            _repo = repo;
        }

        /// <summary>Obtiene todos los teléfonos.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telefono>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        /// <summary>Obtiene un teléfono por número.</summary>
        [HttpGet("{num}")]
        public async Task<ActionResult<Telefono>> GetById(string num)
        {
            var telefono = await _repo.GetByIdAsync(num);
            return telefono == null ? NotFound() : Ok(telefono);
        }

        /// <summary>Registra un nuevo teléfono.</summary>
        [HttpPost]
        public async Task<ActionResult<Telefono>> Create(Telefono telefono)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _repo.ExistsAsync(telefono.Num))
                return BadRequest("Ya existe un teléfono con ese número.");
            var created = await _repo.CreateAsync(telefono);
            return CreatedAtAction(nameof(GetById), new { num = created.Num }, created);
        }

        /// <summary>Actualiza un teléfono.</summary>
        [HttpPut("{num}")]
        public async Task<ActionResult<Telefono>> Update(string num, Telefono telefono)
        {
            var updated = await _repo.UpdateAsync(num, telefono);
            return updated == null ? NotFound() : Ok(updated);
        }

        /// <summary>Elimina un teléfono.</summary>
        [HttpDelete("{num}")]
        public async Task<IActionResult> Delete(string num)
        {
            var deleted = await _repo.DeleteAsync(num);
            return deleted ? NoContent() : NotFound();
        }
    }
}
