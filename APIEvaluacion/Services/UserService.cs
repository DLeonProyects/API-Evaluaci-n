using APIEvaluacion.Data;
using APIEvaluacion.DTOs;
using APIEvaluacion.Models;
using APIEvaluacion.Helpers;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;




namespace APIEvaluacion.Services
{

    /// <Comentarios>
    /// Servicio encargado de manejar la lógica relacionada con los usuarios:
    /// Registro de nuevos usuarios y autenticación mediante JWT.
    /// </Comentarios>
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtHelper _jwtHelper;
        private readonly IValidator<RegisterUserDto> _validator;


        /// Constructor del servicio de usuarios con inyección de dependencias.
        public UserService(ApplicationDbContext context, IJwtHelper jwtHelper, IValidator<RegisterUserDto> validator)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _validator = validator;
        }

        /// RegisterUserAsync: Registra un nuevo usuario en la base de datos in-memory.
        /// Realiza validaciones, encripta la contraseña y genera un token JWT.
        public async Task<(User user, string token)> RegisterUserAsync(RegisterUserDto dto)
        {
            // Ejecutamos la validación manual
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errorMessage = validationResult.Errors.First().ErrorMessage;
                throw new Exception(errorMessage);
            }

            // Verificar si el correo ya está registrado
            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                throw new Exception("El correo ya se encuentra registrado.");
            }

            // Crear la entidad de usuario con la contraseña encriptada mediante BCrypt
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generar token JWT
            var token = _jwtHelper.GenerateJwtToken(user);

            return (user, token);
        }


        /// AuthenticateUserAsync: Autentica (Login) un usuario existente verificando correo y contraseña.
        /// Si las credenciales son correctas, devuelve un token JWT.
        public async Task<string> AuthenticateUserAsync(LoginUserDto dto)
        {
            // Buscar el usuario por correo
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                throw new Exception("Correo o contraseña incorrectos.");
            }

            // Verificar que la contraseña proporcionada coincida
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                throw new Exception("Correo o contraseña incorrectos.");
            }

            // Generar y devolver token JWT
            var token = _jwtHelper.GenerateJwtToken(user);

            return token;
        }


    }
}
