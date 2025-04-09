namespace APIEvaluacion.DTOs
{

    /// <Comentarios>
    /// DTO utilizado para la creación de nuevos usuarios.
    /// Contiene la información que el cliente debe enviar para registrar un usuario.
    /// </Comentarios>
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
