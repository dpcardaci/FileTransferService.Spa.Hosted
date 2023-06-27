using FileTransferService.Spa.Hosted.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FileTransferService.Spa.Hosted.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public TestController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Testing";
        }
    }
}
