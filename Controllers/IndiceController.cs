using CorrecaoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CorrecaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndiceController : ControllerBase
    {
        private const int AnoInicioSerieInpc = 1979;
        private const int MesInicioSerieInpc = 3;

        private readonly ILogger<IndiceController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndiceController(ILogger<IndiceController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "inpc")]
        public async Task<IActionResult> GetInpcAsync([FromQuery] int ano, [FromQuery] int mes)
        {
            var dataMinInpc = new DateTime(AnoInicioSerieInpc, MesInicioSerieInpc, 1);
            var dataInformada = new DateTime(ano, mes, 1);

            if (dataInformada < dataMinInpc)
                return BadRequest("Índices INPC disponíveis a partir de Abril de 1979.");
            
            var client = _httpClientFactory.CreateClient("ApiSidra");
            var indices = await client.GetFromJsonAsync<List<IndiceDto>>($"/values/t/1736/p/{ano:0000}{mes:00}/n1/1/v/44/f/n/h/n?formato=json");

            if (indices == null || indices.Count == 0)
                return BadRequest(new { Sucesso = false, Mensagem = $"Nenhum índice INPC encontrado para o periodo: {ano:0000}{mes:00}." });

            return Ok(indices);
        }
    }
}