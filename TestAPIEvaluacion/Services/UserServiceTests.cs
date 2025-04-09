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
    /// Pruebas unitarias que validan el registro de usuarios y sus reglas de negocio.
    /// </Comentarios>
    public class UserServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IJwtHelper> _jwtHelperMock;
        private readonly Mock<IValidator<RegisterUserDto>> _validatorMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Base de datos en memoria para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
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

            // Por defecto, que la validación sea exitosa para que no interfiera
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Instanciamos el servicio con los mocks
            _userService = new UserService(_context, _jwtHelperMock.Object, _validatorMock.Object);
        }




        /// <Comentarios>
        /// Prueba que verifica registro exitoso con datos válidos.
        /// </Comentarios>


        [Fact]
        public async Task RegisterUserAsync_WithValidData_ShouldCreateUserAndReturnToken()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Name = "Daniel",
                Email = "danieldeleon@example.com",
                Password = "Password123!"
            };

            // Act
            var result = await _userService.RegisterUserAsync(dto);

            // Assert
            Assert.NotNull(result.user);
            Assert.Equal(dto.Name, result.user.Name);
            Assert.Equal(dto.Email, result.user.Email);
            Assert.False(string.IsNullOrWhiteSpace(result.token));
        }



        /// <Comentarios>
        ///  Prueba que verifica que se lance una excepción si el campo de nombre está vacío.
        /// </Comentarios>

        [Fact]
        public async Task RegisterUserAsync_WithEmptyName_ShouldThrowException()
        {
            // Arrange: Creamos un DTO con el nombre vacío
            var dto = new RegisterUserDto
            {
                Name = "",
                Email = "daniel@example.com",
                Password = "Password123!"
            };

            // Simulamos la validación fallida
            _validatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure("Name", "El campo 'Nombre' no debe estar vacío.")
                }));

      
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.RegisterUserAsync(dto));
            Assert.Equal("El campo 'Nombre' no debe estar vacío.", exception.Message);
        }




        /// <Comentarios>
        /// Prueba que verifica que se lance una excepción si el correo no tiene formato válido.
        /// </Comentarios>

        [Fact]
        public async Task RegisterUserAsync_WithInvalidEmail_ShouldThrowException()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Name = "Daniel",
                Email = "correo-no-valido",
                Password = "Password123!"
            };

            // Simulamos la validación fallida
            _validatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure("Email", "El correo electrónico no tiene un formato válido.")
                }));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.RegisterUserAsync(dto));
            Assert.Equal("El correo electrónico no tiene un formato válido.", exception.Message);
        }

        /// <Comentarios>
        /// Prueba que verifica que se lance una excepción si la contraseña no cumple la política de seguridad.
        /// </Comentarios>
        [Fact]
        public async Task RegisterUserAsync_WithWeakPassword_ShouldThrowException()
        {
            // Arrange
            var dto = new RegisterUserDto
            {
                Name = "Daniel",
                Email = "daniel@example.com",
                Password = "12345678"
            };

            // Simulamos la validación fallida
            _validatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure("Password", "La contraseña debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas, números y símbolos.")
                }));

            
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.RegisterUserAsync(dto));
            Assert.Equal("La contraseña debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas, números y símbolos.", exception.Message);
        }
    }
}
