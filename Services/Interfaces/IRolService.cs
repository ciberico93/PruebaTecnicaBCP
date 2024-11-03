using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.Services.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> GetAllRolsAsync();
    }
}
