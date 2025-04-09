namespace APIEvaluacion.DTOs
{

    /// <Comentarios>
    /// DTO utilizado para la autenticación de usuarios.
    /// Contiene las credenciales que el cliente debe proporcionar para iniciar sesión.
    /// </Comentarios>

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
