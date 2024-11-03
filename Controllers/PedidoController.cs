using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services;
using PruebaTecnicaBcp.Services.Interfaces;
using PruebaTecnicaBcp.ViewModel;
using System.Security.Claims;

namespace PruebaTecnicaBcp.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IPedidoService _pedidoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IPedidoDetalleService _pedidoDetalleService;
        private readonly IEstadoPedidoService _estadoPedidoService;

        public PedidoController(IProductoService productoService, IPedidoService pedidoService, IUsuarioService usuarioService, IPedidoDetalleService pedidoDetalleService, IEstadoPedidoService estadoPedidoService)
        {
            _productoService = productoService;
            _pedidoService = pedidoService;
            _usuarioService = usuarioService;
            _pedidoDetalleService = pedidoDetalleService;
            _estadoPedidoService = estadoPedidoService;
        }

        public async Task<IActionResult> MostrarProductos()
        {
            var listaProductos = await _productoService.GetAllProductosAsync();
            return Json(listaProductos);
        }

        public async Task<IActionResult> MostrarDelivery()
        {
            var listaUsuarios = await _usuarioService.GetAllDeliveryAsync();
            return Json(listaUsuarios);
        }

        public async Task<IActionResult> FrmPedido()
        {
            var rolId = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (rolId == "3")
            {
                var listaPedidos = await _pedidoService.GetAllPedidosByRolAsync(rolId, userId);
                return View(listaPedidos);
            } else
            {
                var listaPedidos = await _pedidoService.GetAllPedidosAsync();
                return View(listaPedidos);
            }
            
        }

        public IActionResult FrmNuevoPedido()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FrmNuevoPedido([FromBody] PedidoViewModel oPedidoVM)
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            var userId = claimUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = claimUser.Identity?.Name;
            var usuario = await _usuarioService.GetUsuarioByIdAsync(int.Parse(userId));

            oPedidoVM.oPedido.IdVendedor = usuario.IdUsuario;

            // Validar que haya al menos un detalle de venta
            if (oPedidoVM.PedidoDetalle == null || !oPedidoVM.PedidoDetalle.Any())
            {
                ModelState.AddModelError("", "Debe agregar al menos un detalle de venta.");
                // Retornar la vista con el modelo para que el usuario pueda corregirlo
                return View(oPedidoVM);
            }

            // Obtener el último número de pedido y generar el siguiente en formato P001
            int ultimoNroPedido = await _pedidoService.ObtenerUltimoNroPedidoAsync();
            string nuevoNroPedido = $"P{(ultimoNroPedido + 1).ToString("D3")}"; // Formato P001

            // Crear la entidad de venta
            var pedido = new Pedido
            {
                NroPedido = nuevoNroPedido,
                FechaPedido = DateTime.Now,
                FechaDespacho = null,
                FechaEntrega = null,
                IdVendedor = oPedidoVM.oPedido.IdVendedor,
                IdDelivery = oPedidoVM.oPedido.IdDelivery,
                Total = oPedidoVM.oPedido.Total,
                IdEstadoPedido = 1,
                IdEstado = 1,
            };

            // Añadir los detalles de venta
            foreach (var dv in oPedidoVM.PedidoDetalle)
            {
                pedido.PedidoDetalles.Add(new PedidoDetalle
                {
                    IdProducto = dv.IdProducto,
                    Cantidad = dv.Cantidad,
                    TotalPrecio = dv.TotalPrecio
                });
            }

            // Crear la venta y obtener el VentaID
            int ventaID = await _pedidoService.CreatePedidoServiceAsync(pedido);

            // Retornar una respuesta JSON indicando que la venta se registró correctamente
            return Json(new { respuesta = true, VentaID = ventaID });
        }

        public async Task<IActionResult> FrmDetallePedido(int id)
        {
            var listarDetalles = await _pedidoService.GetPedidoServiceAsyncById(id);
            return View(listarDetalles);
        }

        public async Task<IActionResult> FrmEditarPedido(int id)
        {
            var listarDetalles = await _pedidoService.GetPedidoServiceAsyncById(id);
            ViewBag.ListaEstadoPedido = await _estadoPedidoService.GetAllEstadoPedidoAsync();
            return View(listarDetalles);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPedido(Pedido objPedido)
        {
            if (objPedido == null)
            {
                return Json(new { success = false, message = "El pedido no puede ser nulo" });
            }

            // Llamar al servicio de actualización de pedido
            var resultado = await _pedidoService.UpdatePedidoAsync(objPedido);

            if (resultado == null)
            {
                return Json(new { success = false, message = "No se encontró el pedido para actualizar." });
            }

            return Json(new { success = true });
        }
    }
}
