using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaTecnicaBcp.Context;
using PruebaTecnicaBcp.Models;
using PruebaTecnicaBcp.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PruebaTecnicaBcp.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly PruebaTecnicaBcpContext _context;

        public UsuarioService(PruebaTecnicaBcpContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios
                         .Include(u =>u.IdRolNavigation)
                         .Where(p => p.IdEstado == 1) // Excluye los "eliminados"
                         .ToListAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            // Encriptar la contraseña antes de guardar
            string contrasenaEncriptada = EncriptarContrasena(usuario.Contrasena);

            // Crear una instancia del nuevo usuario con los datos necesarios
            Usuario nuevoUsuario = new Usuario()
            {
                CodigoTrabajador = usuario.CodigoTrabajador,
                Nombre = usuario.Nombre,
                Contrasena = contrasenaEncriptada,
                Email = usuario.Email,
                TelefonoCelular = usuario.TelefonoCelular,
                Puesto = usuario.Puesto,
                IdRol = usuario.IdRol,
                IdEstado = 1 // Estado 1 indica "activo"
            };

            EntityEntry<Usuario>  result = await _context.AddAsync(nuevoUsuario);
            await _context.SaveChangesAsync();

            return result.Entity;
        }


        public async Task<Usuario> UpdateUsuarioAsync(Usuario usuario)
        {
            // Buscar el usuario existente en la base de datos
            var usuarioExistente = await GetUsuarioByIdAsync(usuario.IdUsuario);
            if (usuarioExistente == null)
                return null;

            // Actualizar las propiedades necesarias
            usuarioExistente.CodigoTrabajador = usuario.CodigoTrabajador;
            usuarioExistente.Nombre = usuario.Nombre;
            // Solo encriptar y actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(usuario.Contrasena))
            {
                usuarioExistente.Contrasena = EncriptarContrasena(usuario.Contrasena); // Encriptar la nueva contraseña
            }
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.TelefonoCelular = usuario.TelefonoCelular;
            usuarioExistente.Puesto = usuario.Puesto;
            usuarioExistente.IdRol = usuario.IdRol;

            await _context.SaveChangesAsync();
            return usuarioExistente;

        }
        public async Task DeleteUsuarioAsync(int id)
        {
            // Buscar el usuario en la base de datos
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                // Cambiar el estado para marcarlo como "eliminado"
                usuario.IdEstado = 0; // Estado 0 indica "eliminado"
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
        }




        // Implementación del método de autenticación
        public async Task<Usuario> AuthenticateAsync(string email, string clave)
        {
            string claveEncriptada = EncriptarContrasena(clave);
            // Buscar el usuario con el nombre de usuario y contraseña coincidentes, y con estado activo
            var usuario = await _context.Usuarios
                                        .Where(u => u.Email == email && u.Contrasena == claveEncriptada && u.IdEstado == 1)
                                        .SingleOrDefaultAsync();

            return usuario;
        }

        public string EncriptarContrasena(string clave)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la clave a un arreglo de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(clave));

                // Convertir el arreglo de bytes a un string en hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<IEnumerable<Usuario>> GetAllDeliveryAsync()
        {
            return await _context.Usuarios
                         .Where(p => p.IdEstado == 1 && p.IdRol == 3) 
                         .ToListAsync();
        }
    }
}
