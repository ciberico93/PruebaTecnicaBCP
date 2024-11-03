using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBcp.Context;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services.Interfaces;

namespace PruebaTecnicaBcp.Services
{
    public class RolService : IRolService
    {
        private readonly PruebaTecnicaBcpContext _context;

        public RolService(PruebaTecnicaBcpContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetAllRolsAsync()
        {
            return await _context.Rols.ToListAsync();
        }
    }
    
}
