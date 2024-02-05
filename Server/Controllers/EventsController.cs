using FileTransferService.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Text.Json;
using System.Net;
using FileTransferService.Spa.Hosted.Server.Services;

namespace FileTransferService.Spa.Hosted.Server.Services;

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

    [HttpPost("Upload/Initiated")]
    public async Task<IActionResult> UploadInitiated([FromBody] TransferInfo transferInfo)
    {
        await _eventsService.SendUploadInitiatedEventAsync(transferInfo);
        return Ok();
    }

    [HttpPost("Upload/Completed")]
    public async Task<IActionResult> UploadCompleted([FromBody] TransferInfo transferInfo)
    {
        await _eventsService.SendUploadCompletedEventAsync(transferInfo);
        return Ok();
    }

    [HttpPost("Upload/Error")]
    public async Task<IActionResult> UploadError([FromBody] TransferError transferError)
    {
        await _eventsService.SendUploadErrorEventAsync(transferError);
        return Ok();
    }        

}
