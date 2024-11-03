using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBcp.Context;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services.Interfaces;

namespace PruebaTecnicaBcp.Services
{
    public class EstadoPedidoService : IEstadoPedidoService
    {
        private readonly PruebaTecnicaBcpContext _context;

        public EstadoPedidoService(PruebaTecnicaBcpContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EstadoPedido>> GetAllEstadoPedidoAsync()
        {
           return await _context.EstadoPedidos.ToListAsync();
        }
    }
}
