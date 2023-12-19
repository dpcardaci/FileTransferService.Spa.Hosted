
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
    }
}