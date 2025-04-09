using APIEvaluacion.Models;
using Microsoft.EntityFrameworkCore;


namespace APIEvaluacion.Data
{

    /// <Comentarios>
    /// DbContext de la aplicación.
    /// Representa la sesión con la base de datos InMemory mediante Entity Framework Core.
    /// </Comentarios>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// Tabla virtual de usuarios.
        /// Representa la colección de usuarios dentro de la base de datos.
        public DbSet<User> Users { get; set; }
    }
}
