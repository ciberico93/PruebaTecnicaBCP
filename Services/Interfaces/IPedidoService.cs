using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetAllPedidosAsync();
        Task<int> CreatePedidoServiceAsync(Pedido pedido);
        Task<Pedido> GetPedidoServiceAsyncById(int id);
        Task<int> ObtenerUltimoNroPedidoAsync();
        Task<Pedido> UpdatePedidoAsync(Pedido objPedido);
        Task<List<Pedido>> GetAllPedidosByRolAsync(string rolId, int userId);
    }
}
