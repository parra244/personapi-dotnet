using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Interfaces
{
    public interface IProfesionRepository
    {
        Task<IEnumerable<Profesion>> GetAllAsync();
        Task<Profesion?> GetByIdAsync(int id);
        Task<Profesion> CreateAsync(Profesion profesion);
        Task<Profesion?> UpdateAsync(int id, Profesion profesion);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
