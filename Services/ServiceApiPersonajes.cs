using MvcNetCorePersonajesAWS.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcNetCorePersonajesAWS.Services
{
    public class ServiceApiPersonajes
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServiceApiPersonajes(IConfiguration configuration) {
            this.UrlApi = configuration.GetValue<string>
            ("ApiUrls:ApiPersonajes");
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<List<Personaje>> GetPersonajesAsync() {
            using (HttpClientHandler handler = new HttpClientHandler()) {
                handler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, sslPolicies) =>
                {
                    return true;
                };
                using (HttpClient client = new HttpClient(handler)) {
                    string request = "api/personajes";
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(this.Header);
                    HttpResponseMessage response = await
                    client.GetAsync(this.UrlApi + request);
                    if (response.IsSuccessStatusCode) {
                        List<Personaje> personajes = await
                        response.Content.ReadAsAsync<List<Personaje>>();
                        return personajes;
                    }
                    else {
                        return null;
                    }
                }
            }
        }


        public async Task CreatePersonajeAsync(string nombre, string imagen) {
            using (HttpClient client = new HttpClient()) {
                string request = "api/personajes";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Personaje personaje = new Personaje
                {
                    IdPersonaje = 0,
                    Nombre = nombre,
                    Imagen = imagen
                };
                string json = JsonConvert.SerializeObject(personaje);
                StringContent content = new StringContent
                (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                await client.PostAsync(this.UrlApi + request, content);
            }
        }
    }
}
