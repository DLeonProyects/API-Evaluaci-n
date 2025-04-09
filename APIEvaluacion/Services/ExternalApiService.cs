using System.Text.Json;
using System.Net.Http.Headers;



namespace APIEvaluacion.Services
{
    /// <Comentarios>
    /// Servicio responsable de interactuar con APIs externas.
    /// En este caso, consumimos la API pública de jsonplaceholder.typicode.com
    /// para realizar operaciones GET y POST sobre recursos de "posts".
    /// </Comentarios>
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;

        //Mi ApiURL esta declarada en el appsettings
        private readonly string _externalApiUrl;

        /// Constructor que inyecta una instancia de HttpClient.
        public ExternalApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _externalApiUrl = configuration.GetSection("ExternalApi")["BaseUrl"];
        }




        /// GetPostsAsync: Realiza una solicitud GET para obtener la lista de posts
        /// desde la API externa jsonplaceholder.typicode.com.
        public async Task<object> GetPostsAsync()
        {
            // Enviamos la solicitud GET a la API externa
            var response = await _httpClient.GetAsync(_externalApiUrl);


            // Si la respuesta no es exitosa, lanza una excepción
            response.EnsureSuccessStatusCode();

            // Leemos el contenido de la respuesta como string
            var content = await response.Content.ReadAsStringAsync();

            // Deserializamos el contenido a un objeto dinámico
            return JsonSerializer.Deserialize<object>(content);
        }




        /// CreatePostAsync: Realiza una solicitud POST para crear un nuevo post
        /// en la API externa jsonplaceholder.typicode.com.
        public async Task<object> CreatePostAsync(object postData)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(postData));
            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Enviamos la solicitud POST a la API externa
            var response = await _httpClient.PostAsync(_externalApiUrl, jsonContent);

            // Si la respuesta no es exitosa, lanza una excepción
            response.EnsureSuccessStatusCode();

            // Leemos el contenido de la respuesta como string
            var content = await response.Content.ReadAsStringAsync();

            // Deserializamos el contenido a un objeto dinámico
            return JsonSerializer.Deserialize<object>(content);
        }
    }

}
