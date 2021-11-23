using CorrecaoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CorrecaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndiceController : ControllerBase
    {
        private readonly ILogger<IndiceController> _logger;

        public IndiceController(ILogger<IndiceController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "inpc")]
        public IEnumerable<IndiceDto> GetInpc([FromQuery] string mesano)
        {
            return new IndiceDto[]
            {
                new IndiceDto()
                {
                     NivelTerritorial = "Brasil",
                     ValorIndice = new decimal(0.60)
                }
            };
        }
    }
}