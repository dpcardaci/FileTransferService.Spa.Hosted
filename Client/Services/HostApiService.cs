using FileTransferService.Core;
using FileTransferService.Spa.Hosted.Shared;
using System.Net;
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

        public async Task<TransferEventsDocument[]?> GetTransferEventsAsync(string username)
        {
            username = WebUtility.UrlEncode(username);
            return await _httpClient.GetFromJsonAsync<TransferEventsDocument[]>($"Events?username={username}");
        }

        public async Task SendUploadInitiatedEventAsync(TransferInfo transferInfo)
        {
            await _httpClient.PostAsJsonAsync<TransferInfo>("Events/Upload/Initiated", transferInfo);
        }

        public async Task SendUploadCompletedEventAsync(TransferInfo transferInfo)
        {
            await _httpClient.PostAsJsonAsync<TransferInfo>("Events/Upload/Completed", transferInfo);
        }

        public async Task SendUploadErrorEventAsync(TransferError transferError)
        {
            await _httpClient.PostAsJsonAsync<TransferError>("Events/Upload/Error", transferError);
        }
    }
}
