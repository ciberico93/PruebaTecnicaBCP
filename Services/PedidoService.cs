using PruebaTecnicaBcp.Context;
using PruebaTecnicaBcp.Services.Interfaces;
using PruebaTecnicaBcp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PruebaTecnicaBcp.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly PruebaTecnicaBcpContext _context;
        private readonly IProductoService _productoService;

        public PedidoService(PruebaTecnicaBcpContext context, IProductoService productoService)
        {
            _context = context;
            _productoService = productoService;
        }

        public async Task<int> CreatePedidoServiceAsync(Pedido pedido)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Agregar el pedido
                    await _context.Pedidos.AddAsync(pedido);
                    await _context.SaveChangesAsync(); // Guardar el pedido primero para obtener su ID

                    foreach (var detalle in pedido.PedidoDetalles)
                    {
                        var tproducto = await _productoService.GetProductoByIdAsync(detalle.IdProducto.Value);
                        if (tproducto == null)
                        {
                            throw new InvalidOperationException($"Producto con ID: {detalle.IdProducto} no encontrado");
                        }

                        var cantidadRestante = detalle.Cantidad ?? 0;
                        if (tproducto.Stock < cantidadRestante) // Verificar si hay suficiente stock
                        {
                            throw new InvalidOperationException($"No hay suficiente stock para el producto con ID: {detalle.IdProducto}");
                        }

                        // Actualizar stock del producto
                        tproducto.Stock -= cantidadRestante; // Restar la cantidad del stock
                        _context.Productos.Update(tproducto); // No olvides agregar el producto al contexto

                        // Agregar detalles del pedido a la base de datos
                        //detalle.IdPedido = pedido.IdPedido; // Asignar el ID del pedido a cada detalle
                        //await _context.PedidoDetalles.AddAsync(detalle); // Agregar detalle
                    }

                    await _context.SaveChangesAsync(); // Guardar todos los cambios en la base de datos
                    await transaction.CommitAsync(); // Confirmar la transacción

                    return pedido.IdPedido; // Retorna el ID del pedido recién creado
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Deshacer cambios en caso de error
                                                       // Log o manejo del error según sea necesario
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidosAsync()
        {
            return await _context.Pedidos
                .Include(p => p.IdVendedorNavigation)
                .Include(p => p.IdDeliveryNavigation)
                .Include(p => p.IdEstadoPedidoNavigation)
                .ToListAsync();
        }

        public async Task<List<Pedido>> GetAllPedidosByRolAsync(string rolId, int userId)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.IdVendedorNavigation)
                .Include(p => p.IdDeliveryNavigation)
                .Include(p => p.IdEstadoPedidoNavigation)
                .ToListAsync();

            if (rolId == "3") // Delivery
            {
                // Filtra los pedidos para el rol de Delivery
                pedidos = pedidos.Where(p => p.IdDelivery == userId).ToList();
            }
            else if (rolId == "2") // Vendedor
            {
                // Filtra los pedidos para el rol de Vendedor
                pedidos = pedidos.Where(p => p.IdVendedor == userId).ToList();
            }

            return pedidos;
        }

        public async Task<Pedido> GetPedidoServiceAsyncById(int id)
        {
            return await _context.Pedidos
                .Include(p => p.IdVendedorNavigation)
                .Include(p => p.IdDeliveryNavigation)
                .Include(p => p.IdEstadoPedidoNavigation)
                .Include(p => p.PedidoDetalles)
                    .ThenInclude(pd => pd.IdProductoNavigation) // Incluir la navegación hacia Producto
                .FirstOrDefaultAsync(p => p.IdPedido == id);
        }

        public async Task<int> ObtenerUltimoNroPedidoAsync()
        {
            var ultimoPedido = await _context.Pedidos
            .OrderByDescending(p => p.NroPedido)
            .FirstOrDefaultAsync();

            if (ultimoPedido != null && !string.IsNullOrEmpty(ultimoPedido.NroPedido))
            {
                // Extraer el número eliminando el prefijo "P" y convertirlo a entero
                string numeroSinPrefijo = ultimoPedido.NroPedido.Substring(1);
                if (int.TryParse(numeroSinPrefijo, out int numeroPedido))
                {
                    return numeroPedido;
                }
            }

            return 0; // Retorna 0 si no hay ningún pedido registrado o en caso de error
        }

        public async Task<Pedido> UpdatePedidoAsync(Pedido objPedido)
        {
            var pedidoExiste = await GetPedidoServiceAsyncById(objPedido.IdPedido);
            if (pedidoExiste == null)
            {
                return null;
            }

            // Lógica para actualizar el pedido según el rol
            if (objPedido.IdEstadoPedido == 2) // Rol de Vendedor
            {
                // Solo el vendedor puede actualizar el estado del pedido
                pedidoExiste.IdEstadoPedido = 2;
                pedidoExiste.FechaDespacho = DateTime.Now; // O puedes establecer esto solo si es relevante
            }
            else if (objPedido.IdEstadoPedido == 3) // Rol de Delivery
            {
                pedidoExiste.IdEstadoPedido = 3; // Estado despachado, por ejemplo
                pedidoExiste.FechaEntrega = DateTime.Now; // Fecha de despacho
            }

            // Puedes agregar más condiciones para otros roles según sea necesario

            await _context.SaveChangesAsync();
            return pedidoExiste;
        }
    }
}
