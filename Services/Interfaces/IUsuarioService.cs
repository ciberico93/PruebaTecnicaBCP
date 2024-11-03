using PruebaTecnicaBcp.Models;

namespace PruebaTecnicaBcp.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();

        Task<IEnumerable<Usuario>> GetAllDeliveryAsync();
        Task<Usuario> GetUsuarioByIdAsync(int id);
        Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        Task<Usuario> UpdateUsuarioAsync(Usuario usuario);
        Task DeleteUsuarioAsync(int id);

        // Métodos de autenticación
        Task<Usuario> AuthenticateAsync(string email, string clave);
        string EncriptarContrasena(string clave);
    }
}
