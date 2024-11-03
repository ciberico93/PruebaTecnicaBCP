using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services;
using PruebaTecnicaBcp.Services.Interfaces;

namespace PruebaTecnicaBcp.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        private readonly IRolService _rolService;
        public UsuarioController(IUsuarioService usuarioServicio, IRolService rolService)
        {
            _usuarioServicio = usuarioServicio;
            _rolService = rolService;
        }
        public async Task<IActionResult> FrmUsuario()
        {
            var listarUsuarios = await _usuarioServicio.GetAllUsuariosAsync();
            return View(listarUsuarios);
        }

        public async Task<IActionResult> FrmNuevoUsuario()
        {
            var listaRoles = await _rolService.GetAllRolsAsync();
            ViewBag.ListaRoles = listaRoles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NuevoUsuario(Usuario objUsuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioServicio.CreateUsuarioAsync(objUsuario);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Hubo un error al agregar el usuario" });
        }

        public async Task<IActionResult> FrmEditarUsuario(int id)
        {
            var usuario = await _usuarioServicio.GetUsuarioByIdAsync(id);

            var listaRoles = await _rolService.GetAllRolsAsync();
            ViewBag.ListaRoles = listaRoles;

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarUsuario(Usuario objUsuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioServicio.UpdateUsuarioAsync(objUsuario);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Hubo un error al actualizar el cliente" });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            if (ModelState.IsValid)
            {
                await _usuarioServicio.DeleteUsuarioAsync(id);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Hubo un error al eliminar el usuario" });
        }
    }
}
