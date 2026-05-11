using Microsoft.EntityFrameworkCore;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonaDbContext _context;

        public PersonaRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Estudios)
                    .ThenInclude(e => e.Profesion)
                .ToListAsync();
        }

        public async Task<Persona?> GetByIdAsync(long cc)
        {
            return await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Estudios)
                    .ThenInclude(e => e.Profesion)
                .FirstOrDefaultAsync(p => p.Cc == cc);
        }

        public async Task<Persona> CreateAsync(Persona persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
            return persona;
        }

        public async Task<Persona?> UpdateAsync(long cc, Persona persona)
        {
            var existing = await _context.Personas.FindAsync(cc);
            if (existing == null) return null;

            existing.Nombre   = persona.Nombre;
            existing.Apellido = persona.Apellido;
            existing.Genero   = persona.Genero;
            existing.Edad     = persona.Edad;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(long cc)
        {
            var persona = await _context.Personas.FindAsync(cc);
            if (persona == null) return false;

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long cc)
        {
            return await _context.Personas.AnyAsync(p => p.Cc == cc);
        }
    }
}
