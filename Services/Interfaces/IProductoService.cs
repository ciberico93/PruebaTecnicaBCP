using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<Producto> GetProductoByIdAsync(int id);
        Task<Producto> CreateProductoAsync(Producto producto);
        Task<Producto> UpdateProductoAsync(Producto producto);
        Task DeleteProductoAsync(int id);

        // Método adicional para buscar productos por nombre
        Task<IEnumerable<Producto>> GetProductosByNameAsync(string nombre);
    }
}
