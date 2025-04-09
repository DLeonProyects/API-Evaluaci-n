using APIEvaluacion.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace APIEvaluacion.Helpers
{

    /// <Comentarios>
    /// Implementación de IJwtHelper que se encarga de generar tokens JWT
    /// utilizando las configuraciones definidas en appsettings.json.
    /// </Comentarios>
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;

        /// Constructor que inyecta la configuración de la aplicación.
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// Genera un token JWT basado en los datos del usuario proporcionado.
        /// Incluye claims estándar como Id, Email y Name.
        public string GenerateJwtToken(User user)
        {
            // Obtenemos la configuración JWT desde appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);


            // Definimos los claims que se incluirán en el token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                }),

                // Configuracion de la expiración del token
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),


                // Configuracion del emisor y audiencia desde configuración
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],


                // Configuracion de las credenciales de firma del token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Generamos y escribimos el token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
