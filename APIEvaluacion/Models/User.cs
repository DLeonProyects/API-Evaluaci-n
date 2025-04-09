using System;

/// User: Este modelo representa la entidad de Usuario dentro del sistema.
/// Esta clase define las propiedades que se almacenan para cada usuario.

namespace APIEvaluacion.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
