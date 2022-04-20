using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public HttpCommandDataClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(plat),
            Encoding.UTF8,"apllication/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}",
                httpContent);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST ke CommandService berhasil dikirimkan");
            }
            else
            {
                Console.WriteLine("--> Sync POST ke CommandServices gagal dikirimkan");
            }
        }
    }
}