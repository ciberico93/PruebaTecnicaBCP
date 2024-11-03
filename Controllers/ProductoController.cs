using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services.Interfaces;

namespace PruebaTecnicaBcp.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;
        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public async Task<IActionResult> FrmProducto()
        {
            var listaProductos = await _productoService.GetAllProductosAsync();
            return View(listaProductos);
        }

        public IActionResult FrmNuevoProducto()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoProducto(Producto objProducto)
        {
            if (ModelState.IsValid)
            {
                var producto = await _productoService.CreateProductoAsync(objProducto);
                return RedirectToAction("FrmProducto", "Producto");
            }
            return Json(new { success = false , message = "Hubo un error al agregar el producto" });
        }

        public async Task<IActionResult> FrmEditarProducto(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarProducto(Producto objProducto)
        {
            if (ModelState.IsValid)
            {
                await _productoService.UpdateProductoAsync(objProducto);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Hubo un error al actualizar el cliente" });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            if (ModelState.IsValid)
            {
                await _productoService.DeleteProductoAsync(id);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Hubo un error al eliminar el usuario" });
        }
    }
}
