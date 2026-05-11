using Microsoft.EntityFrameworkCore;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Repositories
{
    public class EstudioRepository : IEstudioRepository
    {
        private readonly PersonaDbContext _context;

        public EstudioRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudio>> GetAllAsync()
        {
            return await _context.Estudios
                .Include(e => e.Profesion)
                .Include(e => e.Persona)
                .ToListAsync();
        }

        public async Task<Estudio?> GetByIdAsync(int idProf, long ccPer)
        {
            return await _context.Estudios
                .Include(e => e.Profesion)
                .Include(e => e.Persona)
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }

        public async Task<Estudio> CreateAsync(Estudio estudio)
        {
            _context.Estudios.Add(estudio);
            await _context.SaveChangesAsync();
            return estudio;
        }

        public async Task<Estudio?> UpdateAsync(int idProf, long ccPer, Estudio estudio)
        {
            var existing = await _context.Estudios
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
            if (existing == null) return null;

            existing.Fecha  = estudio.Fecha;
            existing.Univer = estudio.Univer;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int idProf, long ccPer)
        {
            var estudio = await _context.Estudios
                .FirstOrDefaultAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
            if (estudio == null) return false;

            _context.Estudios.Remove(estudio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int idProf, long ccPer)
        {
            return await _context.Estudios
                .AnyAsync(e => e.IdProf == idProf && e.CcPer == ccPer);
        }
    }
}
