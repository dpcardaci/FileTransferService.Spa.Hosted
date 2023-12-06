using FileTransferService.Core;
using FileTransferService.Spa.Hosted.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace FileTransferService.Spa.Hosted.Client.Services
{
    public class HostApiService : IHostApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HostApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpClient = _httpClientFactory.CreateClient("FileTransferService.Spa.Hosted.ServerAPI");
        }
        public async Task<AppSettings?> GetAppSettingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<AppSettings>("AppSettings");  
        }

        public async Task<TransferEventsDocument[]?> GetTransferEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<TransferEventsDocument[]>("Events");
        }
    }
}
