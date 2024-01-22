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
        Task<TransferEventsDocument[]?> GetTransferEventsAsync(string username);

        /// <summary>
        /// Get AppSettings object
        /// </summary>
        /// <returns>AppSettings object</returns>
        Task<AppSettings?> GetAppSettingsAsync();

        /// <summary>
        /// Send upload initiated event
        /// </summary>
        Task SendUploadInitiatedEventAsync(TransferInfo transferInfo);

        /// <summary>
        /// Send upload completted event
        /// </summary>
        Task SendUploadCompletedEventAsync(TransferInfo transferInfo);

        /// <summary>
        /// Send upload error event
        /// </summary>
        Task SendUploadErrorEventAsync(TransferError transferError);
    }
}
