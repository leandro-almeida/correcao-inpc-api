using System.Text.Json.Serialization;

namespace CorrecaoApi.Models
{
    public class IndiceDto
    {
        // Formato esperado:
        //[{"NN":"Brasil","MN":"%","V":"0.60","D1N":"abril 2019","D2N":"Brasil","D3N":"INPC - Variação mensal"}]

        // URL https://apisidra.ibge.gov.br/values/t/1736/p/201904/n1/1/v/44/f/n/h/n?formato=json

        [JsonPropertyName("NN")]
        public string NivelTerritorial { get; set; } = string.Empty;

        [JsonPropertyName("MN")]
        public string UnidadeMedida { get; set; } = string.Empty;

        [JsonPropertyName("V")]
        public decimal ValorIndice { get; set; }

        [JsonPropertyName("D1N")]
        public string DescricaoMes { get; set; } = string.Empty;

        [JsonPropertyName("D3N")]
        public string DescricaoIndice { get; set; } = string.Empty;

    }
}