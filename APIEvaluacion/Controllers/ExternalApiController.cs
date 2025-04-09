using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Text.Json;
using APIEvaluacion.Services;

namespace APIEvaluacion.Controllers
{

    /// <Comentarios>
    /// Controlador que expone endpoints para interactuar con la API externa jsonplaceholder.typicode.com.
    /// Estos endpoints requieren autenticación JWT para su consumo.
    /// </Comentarios>

    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Importante: Este decorador asegura que requiera JWT
    public class ExternalApiController : ControllerBase
    {
        private readonly ExternalApiService _externalApiService;


        /// Constructor que inyecta el servicio encargado de las operaciones con la API externa.

        public ExternalApiController(ExternalApiService externalApiService)
        {
            _externalApiService = externalApiService;
        }


        /// Endpoint GET que obtiene todos los posts desde la API externa.
        /// Requiere autenticación mediante token JWT.
        [HttpGet("get")]
        public async Task<IActionResult> GetPostsAsync()
        {
            var data = await _externalApiService.GetPostsAsync();
            return Ok(data);
        }


        /// Endpoint POST que crea un nuevo post en la API externa.
        /// Requiere autenticación mediante token JWT.
        [HttpPost("posts")]
        public async Task<IActionResult> CreatePostAsync([FromBody] object postData)
        {
            var result = await _externalApiService.CreatePostAsync(postData);
            return Ok(result);
        }
    }
}
