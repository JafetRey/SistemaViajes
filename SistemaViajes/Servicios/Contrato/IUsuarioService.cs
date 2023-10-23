using Microsoft.EntityFrameworkCore;
using SistemaViajes.Models;

namespace SistemaViajes.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string correo, string v);
        Task<Usuario> SaveUsuario(Usuario modelo);

        public interface IUsuarioService
        {
            Task<Usuario> GetUsuario(string correo, string clave);
            Task<Usuario> SaveUsuario(Usuario modelo);

        }
    }
}
