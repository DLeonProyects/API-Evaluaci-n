using APIEvaluacion.Data;
using APIEvaluacion.DTOs;
using APIEvaluacion.Helpers;
using APIEvaluacion.Models;
using APIEvaluacion.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace TestAPIEvaluacion.Services
{

    /// <Comentarios>
    /// Pruebas unitarias para verificar el flujo de autenticación de usuarios.
    /// </Comentarios>
    public class UserServiceAuthTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IJwtHelper> _jwtHelperMock;
        private readonly Mock<IValidator<RegisterUserDto>> _validatorMock;
        private readonly UserService _userService;

        public UserServiceAuthTests()
        {
            // Configuramos la base de datos InMemory para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // BD única para el test obvio.
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock del IConfiguration para JwtHelper
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config.GetSection("Jwt")["Key"]).Returns("FakeKey");
            configurationMock.Setup(config => config.GetSection("Jwt")["Issuer"]).Returns("FakeIssuer");
            configurationMock.Setup(config => config.GetSection("Jwt")["Audience"]).Returns("FakeAudience");
            configurationMock.Setup(config => config.GetSection("Jwt")["ExpiresInMinutes"]).Returns("60");

            // Mock del IJwtHelper
            _jwtHelperMock = new Mock<IJwtHelper>();
            _jwtHelperMock.Setup(j => j.GenerateJwtToken(It.IsAny<User>())).Returns("fake-jwt-token");

            // Mock del validador para RegisterUserDto
            _validatorMock = new Mock<IValidator<RegisterUserDto>>();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Instanciamos el servicio con los mocks
            _userService = new UserService(_context, _jwtHelperMock.Object, _validatorMock.Object);
        }



        /// <Comentarios>
        ///  Prueba 1: que verifica la autenticación exitosa con credenciales válidas.
        /// </Comentarios>
        [Fact]
        public async Task AuthenticateUserAsync_WithValidCredentials_ShouldReturnToken()
        {
            // Preparamos los datos de prueba válidos.
            var email = "danieldleon@example.com";
            var password = "Password123!";

            // Creamos un usuario válido en la base de datos
            var user = new User
            {
                Name = "Daniel De Leon",
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            var dto = new LoginUserDto
            {
                Email = email,
                Password = password
            };

            var token = await _userService.AuthenticateUserAsync(dto);

            Assert.False(string.IsNullOrWhiteSpace(token));
            // Salida esperada: El token no debe estar vacío si la autenticación fue exitosa.
            // Nota: Validamos que el flujo completo de autenticación se ejecuta correctamente.

        }



        /// <Comentarios>
        ///  Prueba 2: que verifica que se lance una excepción si el correo no existe.
        /// </Comentarios>
        [Fact]
        public async Task AuthenticateUserAsync_WithNonExistingEmail_ShouldThrowException()
        {
            //Creamos un DTO con un correo que no existe en la base de datos
            var dto = new LoginUserDto
            {
                Email = "nonexistentxdxd@example.com",
                Password = "Password123!"
            };

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.AuthenticateUserAsync(dto));
            Assert.Equal("Correo o contraseña incorrectos.", exception.Message);

            // Salida esperada: La excepción debe indicar que el correo o contraseña son incorrectos.
            // Nota: Esta prueba garantiza que el servicio maneja correctamente intentos de login con correos inexistentes.
        }




        /// <Comentarios>
        ///  Prueba 3: que verifica que se lance una excepción si la contraseña es incorrecta.
        /// </Comentarios>
        [Fact]
        public async Task AuthenticateUserAsync_WithIncorrectPassword_ShouldThrowException()
        {
            // Preparamos los datos de prueba válidos.
            var email = "deleonleon@example.com";
            var correctPassword = "CorrectPassword123!";
            var wrongPassword = "ConstrasenaIncorrecta!";

            // Creamos un usuario válido en la base de datos
            var user = new User
            {
                Name = "Alejandro De Leon",
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(correctPassword)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //Creamos DTO con el correo correcto pero contraseña incorrecta
            var dto = new LoginUserDto
            {
                Email = email,
                Password = wrongPassword
            };

            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.AuthenticateUserAsync(dto));
            Assert.Equal("Correo o contraseña incorrectos.", exception.Message);

            // Salida esperada: La excepción debe indicar que el correo o contraseña son incorrectos.
            // Nota: Esta prueba valida que la verificación de contraseña está funcionando correctamente.
            // Aseguramos que no se permita el acceso con contraseñas incorrectas.
        }
    }
}
