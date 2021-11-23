using CorrecaoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CorrecaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CorrecaoController : ControllerBase
    {
        private readonly ILogger<CorrecaoController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CorrecaoController(ILogger<CorrecaoController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost(Name = "atualizar-valor")]
        public async Task<IActionResult> AtualizarValorAsync([FromBody] CorrecaoRequestDto correcaoRequestDto)
        {
            var anoMesCorrente = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dataAux = new DateTime(correcaoRequestDto.AnoOriginal, correcaoRequestDto.MesOriginal, 1);
            var periodos = string.Empty;

            while (dataAux.Date <= anoMesCorrente.Date)
            {
                periodos += $",{dataAux.Year:0000}{dataAux.Month:00}";
                dataAux = dataAux.AddMonths(1);
            }
            periodos = periodos.TrimStart(',');

            // Obtem indices da API Sidra
            var client = _httpClientFactory.CreateClient("ApiSidra");
            var indices = await client.GetFromJsonAsync<List<IndiceDto>>($"/values/t/1736/p/{periodos}/n1/1/v/44/f/n/h/n?formato=json");

            if (indices == null || indices.Count == 0)
                return BadRequest(new { Sucesso = false, Mensagem = $"Nenhum índice INPC encontrado nos períodos: {periodos}." });

            var indiceAcumulado = indices.Sum(i => i.ValorIndice);

            return Ok(new CorrecaoResponseDto
            {
                ValorOriginal = correcaoRequestDto.ValorOriginal,
                ValorAtualizado = correcaoRequestDto.ValorOriginal * (1 + (indiceAcumulado/100)),
                IndiceAcumulado = indiceAcumulado
            });
        }

        public class CorrecaoRequestDto
        {
            public decimal ValorOriginal { get; set; }
            public int AnoOriginal { get; set; }
            public int MesOriginal { get; set; }
        }

        public class CorrecaoResponseDto
        {
            public decimal ValorOriginal { get; set; }
            public decimal ValorAtualizado { get; set; }
            public decimal IndiceAcumulado { get; set; }
        }
    }
}