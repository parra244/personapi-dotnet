using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Interfaces
{
    public interface ITelefonoRepository
    {
        Task<IEnumerable<Telefono>> GetAllAsync();
        Task<Telefono?> GetByIdAsync(string num);
        Task<Telefono> CreateAsync(Telefono telefono);
        Task<Telefono?> UpdateAsync(string num, Telefono telefono);
        Task<bool> DeleteAsync(string num);
        Task<bool> ExistsAsync(string num);
    }
}
