using FileTransferService.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace FileTransferService.Spa.Hosted.Server.Services
{
    public class EventsService : IEventsService
    {
        private readonly IConfiguration _configuration;
        private readonly CosmosClient _client;
        private readonly Database _database;
        private readonly Container _container;
        public EventsService(IConfiguration configuration) 
        {
            _configuration = configuration;
            _client = new CosmosClient(_configuration["EventDbUri"], _configuration["EventDbReadOnlyKey"]);
            _database = _client.GetDatabase(_configuration["EventDbName"]);
            _container = _database.GetContainer(_configuration["EventDbContainerName"]);
        }

        public async Task<TransferEventsDocument[]?> GetTransferEventAsync(string username)
        {
           
            IOrderedQueryable<TransferEventsDocument> queryable = _container.GetItemLinqQueryable<TransferEventsDocument>();
            int count = await queryable.Where(t => t.OriginatingUserPrincipalName == username).CountAsync();
            TransferEventsDocument[] transferEventsDocuments = new TransferEventsDocument[] { };

            if (count != 0)
            {
                FeedIterator<TransferEventsDocument> feedIterator = queryable.Where(t => t.OriginatingUserPrincipalName == username).OrderByDescending(t => t.OriginationDateTime).ToFeedIterator();

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
    }
}