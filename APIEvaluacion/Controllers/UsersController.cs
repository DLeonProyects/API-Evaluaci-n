using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIEvaluacion.DTOs;
using APIEvaluacion.Services;


namespace APIEvaluacion.Controllers
{

    /// <Comentarios>
    /// Controlador responsable de la gestión de usuarios.
    /// Incluye endpoints para el registro y autenticación de usuarios.
    /// </Comentarios>

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        /// Constructor que inyecta el servicio de usuarios.
        public UsersController(UserService userService)
        {
            _userService = userService;
        }


        /// Endpoint para registrar un nuevo usuario.
        /// Valida los datos recibidos, crea el usuario, genera el token JWT y retorna la información básica del usuario.
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {

                var (user, token) = await _userService.RegisterUserAsync(dto);

                // Retornar respuesta personalizada (sin exponer la contraseña por seguridad)
                return Ok(new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                // En caso de error de validación o registro, se retorna un 400 BadRequest con el mensaje de error
                return BadRequest(new { error = ex.Message });
            }
        }


        /// Endpoint para autenticar un usuario existente.
        /// Verifica las credenciales y devuelve un token JWT si la autenticación es exitosa.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            try
            {
                var token = await _userService.AuthenticateUserAsync(dto);

                return Ok(new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                // En caso de error de autenticación, se retorna un 400 BadRequest con el mensaje de error
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
