using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    // ── MVC Controller ───────────────────────────────────────────────────────
    public class EstudioController : Controller
    {
        private readonly IEstudioRepository   _repo;
        private readonly IPersonaRepository   _personaRepo;
        private readonly IProfesionRepository _profesionRepo;

        public EstudioController(
            IEstudioRepository repo,
            IPersonaRepository personaRepo,
            IProfesionRepository profesionRepo)
        {
            _repo          = repo;
            _personaRepo   = personaRepo;
            _profesionRepo = profesionRepo;
        }

        public async Task<IActionResult> Index()
            => View(await _repo.GetAllAsync());

        public async Task<IActionResult> Details(int idProf, long ccPer)
        {
            var estudio = await _repo.GetByIdAsync(idProf, ccPer);
            return estudio == null ? NotFound() : View(estudio);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSelectListsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estudio estudio)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return View(estudio);
            }
            await _repo.CreateAsync(estudio);
            TempData["Success"] = "Estudio registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int idProf, long ccPer)
        {
            var estudio = await _repo.GetByIdAsync(idProf, ccPer);
            if (estudio == null) return NotFound();
            await LoadSelectListsAsync();
            return View(estudio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idProf, long ccPer, Estudio estudio)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return View(estudio);
            }
            var result = await _repo.UpdateAsync(idProf, ccPer, estudio);
            if (result == null) return NotFound();
            TempData["Success"] = "Estudio actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int idProf, long ccPer)
        {
            var estudio = await _repo.GetByIdAsync(idProf, ccPer);
            return estudio == null ? NotFound() : View(estudio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idProf, long ccPer)
        {
            await _repo.DeleteAsync(idProf, ccPer);
            TempData["Success"] = "Estudio eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSelectListsAsync()
        {
            var personas   = await _personaRepo.GetAllAsync();
            var profesiones = await _profesionRepo.GetAllAsync();

            ViewBag.Personas = new SelectList(
                personas, "Cc", "Nombre");
            ViewBag.Profesiones = new SelectList(
                profesiones, "Id", "Nom");
        }
    }

    // ── API Controller ───────────────────────────────────────────────────────
    [ApiController]
    [Route("api/[controller]")]
    public class EstudioApiController : ControllerBase
    {
        private readonly IEstudioRepository _repo;

        public EstudioApiController(IEstudioRepository repo)
        {
            _repo = repo;
        }

        /// <summary>Obtiene todos los estudios.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetAll()
            => Ok(await _repo.GetAllAsync());

        /// <summary>Obtiene un estudio por clave compuesta (idProf, ccPer).</summary>
        [HttpGet("{idProf}/{ccPer}")]
        public async Task<ActionResult<Estudio>> GetById(int idProf, long ccPer)
        {
            var estudio = await _repo.GetByIdAsync(idProf, ccPer);
            return estudio == null ? NotFound() : Ok(estudio);
        }

        /// <summary>Crea un nuevo estudio.</summary>
        [HttpPost]
        public async Task<ActionResult<Estudio>> Create(Estudio estudio)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _repo.ExistsAsync(estudio.IdProf, estudio.CcPer))
                return BadRequest("Ya existe ese registro de estudio.");
            var created = await _repo.CreateAsync(estudio);
            return CreatedAtAction(nameof(GetById),
                new { idProf = created.IdProf, ccPer = created.CcPer }, created);
        }

        /// <summary>Actualiza un estudio.</summary>
        [HttpPut("{idProf}/{ccPer}")]
        public async Task<ActionResult<Estudio>> Update(int idProf, long ccPer, Estudio estudio)
        {
            var updated = await _repo.UpdateAsync(idProf, ccPer, estudio);
            return updated == null ? NotFound() : Ok(updated);
        }

        /// <summary>Elimina un estudio.</summary>
        [HttpDelete("{idProf}/{ccPer}")]
        public async Task<IActionResult> Delete(int idProf, long ccPer)
        {
            var deleted = await _repo.DeleteAsync(idProf, ccPer);
            return deleted ? NoContent() : NotFound();
        }
    }
}
