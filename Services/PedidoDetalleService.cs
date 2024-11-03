using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBcp.Context;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services.Interfaces;

namespace PruebaTecnicaBcp.Services
{
    public class PedidoDetalleService : IPedidoDetalleService
    {
        private readonly PruebaTecnicaBcpContext _context;

        public PedidoDetalleService(PruebaTecnicaBcpContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDetalle>> GetPedidoDetalleByIdAsync(int id)
        {
            var listarPedido = await _context.PedidoDetalles.Where(p => p.IdPedidoDetalle == id)
                .Include(p => p.IdProductoNavigation)
                .ToListAsync();
            return listarPedido;
        }
    }
}
