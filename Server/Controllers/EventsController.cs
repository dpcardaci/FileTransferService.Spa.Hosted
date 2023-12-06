using FileTransferService.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Text.Json;

namespace FileTransferService.Spa.Hosted.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EventsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EventsController(IConfiguration configuration)
        {
               _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string? username = User.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
            if (username == null) { return BadRequest(); }
            
            CosmosClient cosmosClient = new CosmosClient(_configuration["EventDbUri"], _configuration["EventDbReadOnlyKey"]);
            Database database = cosmosClient.GetDatabase(_configuration["EventDbName"]);
            Container container = database.GetContainer(_configuration["EventDbContainerName"]);

            IOrderedQueryable<TransferEventsDocument> queryable = container.GetItemLinqQueryable<TransferEventsDocument>();
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
            return Ok(JsonSerializer.Serialize(transferEventsDocuments));
        }

    }
}
