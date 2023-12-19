using FileTransferService.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Text.Json;
using System.Net;
using FileTransferService.Spa.Hosted.Server.Services;

namespace FileTransferService.Spa.Hosted.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;
        public EventsController(IEventsService eventsService)
        {
               _eventsService = eventsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string username)
        {
            username = WebUtility.UrlDecode(username);
            TransferEventsDocument[] transferEventsDocuments = await _eventsService.GetTransferEventAsync(username);

            return Ok(JsonSerializer.Serialize(transferEventsDocuments));
        }

    }
}
