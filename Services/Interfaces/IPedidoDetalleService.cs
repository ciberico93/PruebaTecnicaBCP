using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.Services.Interfaces
{
    public interface IPedidoDetalleService
    {
        Task<IEnumerable<PedidoDetalle>> GetPedidoDetalleByIdAsync(int id);
    }
}
