using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    // ── MVC Controller ───────────────────────────────────────────────────────
    public class ProfesionController : Controller
    {
        private readonly IProfesionRepository _repo;

        public ProfesionController(IProfesionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
            => View(await _repo.GetAllAsync());

        public async Task<IActionResult> Details(int id)
        {
            var profesion = await _repo.GetByIdAsync(id);
            return profesion == null ? NotFound() : View(profesion);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profesion profesion)
        {
            if (!ModelState.IsValid) return View(profesion);
            await _repo.CreateAsync(profesion);
            TempData["Success"] = "Profesión creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var profesion = await _repo.GetByIdAsync(id);
            return profesion == null ? NotFound() : View(profesion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profesion profesion)
        {
            if (id != profesion.Id) return BadRequest();
            if (!ModelState.IsValid) return View(profesion);
            var result = await _repo.UpdateAsync(id, profesion);
            if (result == null) return NotFound();
            TempData["Success"] = "Profesión actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var profesion = await _repo.GetByIdAsync(id);
            return profesion == null ? NotFound() : View(profesion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            TempData["Success"] = "Profesión eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }

    // ── API Controller ───────────────────────────────────────────────────────
    [ApiController]
    [Route("api/Profesion")]
    public class ProfesionApiController : ControllerBase
    {
        private readonly IProfesionRepository _repo;

        public ProfesionApiController(IProfesionRepository repo)
        {
            _repo = repo;
        }

        /// <summary>Obtiene todas las profesiones.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profesion>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        /// <summary>Obtiene una profesión por ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Profesion>> GetById(int id)
        {
            var profesion = await _repo.GetByIdAsync(id);
            return profesion == null ? NotFound() : Ok(profesion);
        }

        /// <summary>Crea una nueva profesión.</summary>
        [HttpPost]
        public async Task<ActionResult<Profesion>> Create(Profesion profesion)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _repo.ExistsAsync(profesion.Id))
                return BadRequest("Ya existe una profesión con ese ID.");
            var created = await _repo.CreateAsync(profesion);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>Actualiza una profesión.</summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Profesion>> Update(int id, Profesion profesion)
        {
            if (id != profesion.Id) return BadRequest();
            var updated = await _repo.UpdateAsync(id, profesion);
            return updated == null ? NotFound() : Ok(updated);
        }

        /// <summary>Elimina una profesión.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
