
using FileTransferService.Core;

namespace FileTransferService.Spa.Hosted.Server.Services
{
    public interface IEventsService
    {
        /// <summary>
        /// Get all transfer events for a user
        /// </summary>
        /// <returns>IEnummerable of type TransferEventsDocument objects</returns>
        Task<TransferEventsDocument[]?> GetTransferEventAsync(string username);

        /// <summary>
        /// Send upload initiated event
        /// </summary>
        Task SendUploadInitiatedEventAsync(TransferInfo transferInfo);

        /// <summary>
        /// Send upload completed event
        /// </summary>
        Task SendUploadCompletedEventAsync(TransferInfo transferInfo);

        /// <summary>
        /// Send upload error event
        /// </summary>
        Task SendUploadErrorEventAsync(TransferError transferError);
    }
}