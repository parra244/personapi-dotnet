using Microsoft.EntityFrameworkCore;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.Models;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Repositories
{
    public class TelefonoRepository : ITelefonoRepository
    {
        private readonly PersonaDbContext _context;

        public TelefonoRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Telefono>> GetAllAsync()
        {
            return await _context.Telefonos
                .Include(t => t.Persona)
                .ToListAsync();
        }

        public async Task<Telefono?> GetByIdAsync(string num)
        {
            return await _context.Telefonos
                .Include(t => t.Persona)
                .FirstOrDefaultAsync(t => t.Num == num);
        }

        public async Task<Telefono> CreateAsync(Telefono telefono)
        {
            _context.Telefonos.Add(telefono);
            await _context.SaveChangesAsync();
            return telefono;
        }

        public async Task<Telefono?> UpdateAsync(string num, Telefono telefono)
        {
            var existing = await _context.Telefonos.FindAsync(num);
            if (existing == null) return null;

            existing.Oper   = telefono.Oper;
            existing.Duenio = telefono.Duenio;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string num)
        {
            var telefono = await _context.Telefonos.FindAsync(num);
            if (telefono == null) return false;

            _context.Telefonos.Remove(telefono);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string num)
        {
            return await _context.Telefonos.AnyAsync(t => t.Num == num);
        }
    }
}
