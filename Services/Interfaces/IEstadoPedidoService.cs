using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.Services.Interfaces
{
    public interface IEstadoPedidoService
    {
        Task<IEnumerable<EstadoPedido>> GetAllEstadoPedidoAsync();
    }
}
