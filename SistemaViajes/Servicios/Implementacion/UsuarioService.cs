using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SistemaViajes.Models;
using SistemaViajes.Servicios.Contrato;



namespace SistemaViajes.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly SistemaViajesContext _dbContext;

        public UsuarioService(SistemaViajesContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usario_encontrado = await _dbContext.Usuarios.Where(u => u.Correo == correo && u.Clave == clave)
                                                                 .FirstOrDefaultAsync();

            return usario_encontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            _dbContext.Usuarios.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
