using CorrecaoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CorrecaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndiceController : ControllerBase
    {
        private readonly ILogger<IndiceController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndiceController(ILogger<IndiceController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("inpc")]
        public async Task<IActionResult> GetInpcAsync([FromQuery] int ano, [FromQuery] int mes)
        {
            if (ano <= 0 || mes <= 0)
                return BadRequest("Parametros invalidos.");

            if (!InpcUtil.PossuiIndiceInpc(ano,mes))
                return BadRequest("Índices INPC disponíveis a partir de Abril de 1979.");
            
            var client = _httpClientFactory.CreateClient("ApiSidra");
            var indices = await client.GetFromJsonAsync<List<IndiceInpcSidraDto>>($"/values/t/1736/p/{ano:0000}{mes:00}/n1/1/v/44/f/n/h/n?formato=json");

            if (indices == null || indices.Count == 0)
                return BadRequest(new { Sucesso = false, Mensagem = $"Nenhum índice INPC encontrado para o periodo: {ano:0000}{mes:00}." });

            return Ok(indices);
        }
    }
}