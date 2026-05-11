using Microsoft.EntityFrameworkCore;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Repositories
{
    public class ProfesionRepository : IProfesionRepository
    {
        private readonly PersonaDbContext _context;

        public ProfesionRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesion>> GetAllAsync()
        {
            return await _context.Profesiones.ToListAsync();
        }

        public async Task<Profesion?> GetByIdAsync(int id)
        {
            return await _context.Profesiones
                .Include(p => p.Estudios)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profesion> CreateAsync(Profesion profesion)
        {
            _context.Profesiones.Add(profesion);
            await _context.SaveChangesAsync();
            return profesion;
        }

        public async Task<Profesion?> UpdateAsync(int id, Profesion profesion)
        {
            var existing = await _context.Profesiones.FindAsync(id);
            if (existing == null) return null;

            existing.Nom = profesion.Nom;
            existing.Des = profesion.Des;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profesion = await _context.Profesiones.FindAsync(id);
            if (profesion == null) return false;

            _context.Profesiones.Remove(profesion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Profesiones.AnyAsync(p => p.Id == id);
        }
    }
}
