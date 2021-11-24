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

        [HttpPost]
        [Route("atualizar-valor")]
        public async Task<IActionResult> AtualizarValorAsync([FromBody] CorrecaoRequestDto request)
        {
            if (request.ValorOriginal <= 0 || request.AnoOriginal <= 0 || request.MesOriginal <= 0)
                return BadRequest("Parametros invalidos.");

            if (!InpcUtil.PossuiIndiceInpc(request.AnoOriginal, request.MesOriginal))
                return BadRequest("Índices INPC disponíveis a partir de Abril de 1979.");

            var anoMesAtualizacao = new DateTime(
                request.AnoAtualizacao <= 0 ? DateTime.Now.Year : request.AnoAtualizacao, 
                request.MesAtualizacao <= 0 ? DateTime.Now.Month : request.MesAtualizacao,
                1);
            
            var dataAux = new DateTime(request.AnoOriginal, request.MesOriginal, 1);
            var periodos = string.Empty;

            while (dataAux.Date < anoMesAtualizacao.Date) // Atualiza ate o indice do mes anterior
            {
                periodos += $",{dataAux.Year:0000}{dataAux.Month:00}";
                dataAux = dataAux.AddMonths(1);
            }
            periodos = periodos.TrimStart(',');

            // Obtem indices da API Sidra
            var client = _httpClientFactory.CreateClient("ApiSidra");
            var indices = await client.GetFromJsonAsync<List<IndiceInpcSidraDto>>($"/values/t/1736/p/{periodos}/n1/1/v/44/f/n/h/n?formato=json");

            if (indices == null || indices.Count == 0)
                return BadRequest($"Nenhum índice INPC encontrado nos períodos: {periodos}.");

            // Calcula o Indice Acumulado
            decimal indiceAcumulado = 1;
            indices.ForEach(x => indiceAcumulado *= 1 + (x.ValorIndice / 100));

            return Ok(new CorrecaoResponseDto
            {
                ValorOriginal = request.ValorOriginal,
                ValorAtualizado = request.ValorOriginal * indiceAcumulado,
                IndiceAcumulado = (indiceAcumulado - 1) * 100
            });
        }

        public class CorrecaoRequestDto
        {
            public decimal ValorOriginal { get; set; }
            public int AnoOriginal { get; set; }
            public int MesOriginal { get; set; }
            public int AnoAtualizacao { get; set; }
            public int MesAtualizacao { get; set; }
        }

        public class CorrecaoResponseDto
        {
            public decimal ValorOriginal { get; set; }
            public decimal ValorAtualizado { get; set; }
            public decimal IndiceAcumulado { get; set; }
        }
    }
}