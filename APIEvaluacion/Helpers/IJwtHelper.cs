using APIEvaluacion.Models;

namespace APIEvaluacion.Helpers
{
    /// <Comentarios>
    /// Interfaz para definir las operaciones relacionadas con la generación de tokens JWT.
    /// Permite desacoplar la implementación concreta y facilita las pruebas unitarias.
    /// </Comentarios>
    public interface IJwtHelper
    {
        /// Genera un token JWT para un usuario autenticado.
        string GenerateJwtToken(User user);
    }
}
