using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAL.Interfaces
{
    public interface IEstudioRepository
    {
        Task<IEnumerable<Estudio>> GetAllAsync();
        Task<Estudio?> GetByIdAsync(int idProf, long ccPer);
        Task<Estudio> CreateAsync(Estudio estudio);
        Task<Estudio?> UpdateAsync(int idProf, long ccPer, Estudio estudio);
        Task<bool> DeleteAsync(int idProf, long ccPer);
        Task<bool> ExistsAsync(int idProf, long ccPer);
    }
}
