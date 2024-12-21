using IpGeo.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpGeo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CsvController(CsvService csvService) : ControllerBase
    {
        private readonly CsvService _csvService = csvService;

        [HttpPost("import")]
        public IActionResult ImportCsvData([FromBody] string csvUrl)
        {
            Task.Run(() => _csvService.DownloadAndSaveCsvDataAsync(csvUrl));
            return Accepted();
        }
    }
}
