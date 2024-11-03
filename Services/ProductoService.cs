using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaTecnicaBcp.Context;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services.Interfaces;

namespace PruebaTecnicaBcp.Services
{
    public class ProductoService : IProductoService
    {
        private readonly PruebaTecnicaBcpContext _context;

        public ProductoService(PruebaTecnicaBcpContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            // Obtener todos los productos disponibles
            return await _context.Productos
                .Where(p => p.IdEstado == 1)
                .ToListAsync();
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            // Buscar un producto por IdProducto
            return await _context.Productos.FindAsync(id);
        }

        public async Task<IEnumerable<Producto>> GetProductosByNameAsync(string nombre)
        {
            // Buscar productos cuyo nombre contenga la cadena de búsqueda
            return await _context.Productos
                .Where(p => EF.Functions.Like(p.Nombre, $"%{nombre}%"))
                .ToListAsync();
        }


        public async Task<Producto> CreateProductoAsync(Producto producto)
        {
            Producto nuevoProducto = new Producto()
            {
                Sku = producto.Sku,
                Nombre = producto.Nombre,
                IdTipo = producto.IdTipo,
                Etiqueta = producto.Etiqueta,
                Precio = producto.Precio,
                Stock = producto.Stock
            };

            EntityEntry<Producto> result = await _context.AddAsync(nuevoProducto);
            await _context.SaveChangesAsync();

            return result.Entity;

        }


        public async Task<Producto> UpdateProductoAsync(Producto producto)
        {
            // Buscar el producto existente
            var productoExistente = await GetProductoByIdAsync(producto.IdProducto);
            if (productoExistente == null)
                return null;

            // Actualizar las propiedades necesarias
            productoExistente.Sku = producto.Sku;
            productoExistente.Nombre = producto.Nombre;
            productoExistente.IdTipo = producto.IdTipo;
            productoExistente.Etiqueta = producto.Etiqueta;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;

            await _context.SaveChangesAsync();
            return productoExistente;
        }

        public async Task DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.IdEstado = 0; // Estado 0 indica "eliminado"
                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();
                await _context.SaveChangesAsync();
            }
        }

    }
}