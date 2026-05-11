using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona?> GetByIdAsync(long cc);
        Task<Persona> CreateAsync(Persona persona);
        Task<Persona?> UpdateAsync(long cc, Persona persona);
        Task<bool> DeleteAsync(long cc);
        Task<bool> ExistsAsync(long cc);
    }
}
