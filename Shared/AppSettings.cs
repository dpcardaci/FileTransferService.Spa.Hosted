
namespace FileTransferService.Spa.Hosted.Shared
{
    public class AppSettings
    {
        public string? UploadStorageAccountSasToken { get; set; }
        public string? UploadStorageAccountName { get; set; }
        public string? UploadContainerName { get; set; }
        public string? UploadInitiatedTopicUri { get; set; }
        public string? UploadInitiatedTopicKey { get; set; }
        public string? UploadCompletedTopicUri { get; set; }
        public string? UploadCompletedTopicKey { get; set; }
        public string? UploadErrorTopicUri { get; set; }
        public string? UploadErrorTopicKey { get; set; }
    }
}
