using FileTransferService.Core;
using FileTransferService.Spa.Hosted.Shared;

namespace FileTransferService.Spa.Hosted.Client.Services
{
    public interface IHostApiService
    {
        /// <summary>
        /// Get all transfer events for the current user
        /// </summary>
        /// <returns>IEnummerable of type TransferEventsDocument objects</returns>
        Task<TransferEventsDocument[]?> GetTransferEventsAsync();

        /// <summary>
        /// Get AppSettings object
        /// </summary>
        /// <returns>AppSettings object</returns>
        Task<AppSettings?> GetAppSettingsAsync();
    }
}
