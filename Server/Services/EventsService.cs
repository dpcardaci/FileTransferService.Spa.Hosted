using FileTransferService.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Azure.Messaging.EventGrid;
using Azure;

namespace FileTransferService.Spa.Hosted.Server.Services
{
    public class EventsService : IEventsService
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosClient _client;
        private readonly Database _database;
        private readonly Container _container;
        private readonly EventGridPublisherClient _uploadInitiatedPublisher;
        private readonly EventGridPublisherClient _uploadCompletedPublisher;
        private readonly EventGridPublisherClient _uploadErrorPublisher;

        public EventsService(IConfiguration configuration) 
        {
            _configuration = configuration;
            _client = new CosmosClient(_configuration["EventDbUri"], _configuration["EventDbReadOnlyKey"]);
            _database = _client.GetDatabase(_configuration["EventDbName"]);
            _container = _database.GetContainer(_configuration["EventDbContainerName"]);

            _uploadInitiatedPublisher = new EventGridPublisherClient(
                new Uri(_configuration["UploadInitiatedTopicUri"]),
                new AzureKeyCredential(_configuration["UploadInitiatedTopicKey"]));

            _uploadCompletedPublisher = new EventGridPublisherClient(
                new Uri(_configuration["UploadCompletedTopicUri"]),
                new AzureKeyCredential(_configuration["UploadCompletedTopicKey"]));

            _uploadErrorPublisher = new EventGridPublisherClient(
                new Uri(_configuration["UploadErrorTopicUri"]),
                new AzureKeyCredential(_configuration["UploadErrorTopicKey"]));                
        }

        public async Task<TransferEventsDocument[]?> GetTransferEventAsync(string username)
        {
           
            IOrderedQueryable<TransferEventsDocument> queryable = _container.GetItemLinqQueryable<TransferEventsDocument>();
            int count = await queryable.Where(t => t.OriginatingUserPrincipalName == username || t.OnBehalfOfUserPrincipalName == username).CountAsync();
            TransferEventsDocument[] transferEventsDocuments = new TransferEventsDocument[] { };

            if (count != 0)
            {
                FeedIterator<TransferEventsDocument> feedIterator = queryable.Where(t => t.OriginatingUserPrincipalName == username || t.OnBehalfOfUserPrincipalName == username).OrderByDescending(t => t.OriginationDateTime).ToFeedIterator();

                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<TransferEventsDocument> feedResponse = await feedIterator.ReadNextAsync();
                    
                    foreach(TransferEventsDocument transferEventsDocument in feedResponse)
                    {
                        transferEventsDocuments = transferEventsDocuments.Append(transferEventsDocument).ToArray();
                    }
                }
            }
            return transferEventsDocuments;
        }

        public async Task SendUploadCompletedEventAsync(TransferInfo transferInfo)
        {
                EventGridEvent uploadCompletedEventGridEvent = new EventGridEvent(
                    "FileTransferService/Upload",
                    "Completed",
                    "1.0",
                    transferInfo
                );

                await _uploadCompletedPublisher.SendEventAsync(uploadCompletedEventGridEvent);
        }

        public async Task SendUploadErrorEventAsync(TransferError transferError)
        {
                EventGridEvent uploadCompletedEventGridEvent = new EventGridEvent(
                    "FileTransferService/Upload",
                    "Error",
                    "1.0",
                    transferError
                );

                await _uploadErrorPublisher.SendEventAsync(uploadCompletedEventGridEvent);
        }

        public async Task SendUploadInitiatedEventAsync(TransferInfo transferInfo)
        {
                EventGridEvent uploadInitiatedEventGridEvent = new EventGridEvent(
                    "FileTransferService/Upload",
                    "Initiated",
                    "1.0",
                    transferInfo
                );

                await _uploadInitiatedPublisher.SendEventAsync(uploadInitiatedEventGridEvent);
        }
    }
}