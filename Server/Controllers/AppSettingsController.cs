using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using FileTransferService.Spa.Hosted.Shared;

namespace FileTransferService.Spa.Hosted.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class AppSettingsController : ControllerBase
{
    private readonly ILogger<AppSettingsController> _logger;
    private readonly IConfiguration _configuration;

    public AppSettingsController(ILogger<AppSettingsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public AppSettings Get()
    {
        return new AppSettings
        {
            UploadContainerName = _configuration["UploadContainerName"],
            UploadStorageAccountName = _configuration["UploadStorageAccountName"],
            UploadStorageAccountSasToken = _configuration["UploadStorageAccountSasToken"]
        };
    }
}
