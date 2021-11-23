# Correcao INPC Api
Este projeto é apenas uma PoC (proof of concept) de API para corrigir um valor monetário pelo índice INPC, em reais (R$). Desenvolvi rapidamente (cerca de 1h) para chegar no resultado final desejado, dado um valor e uma data passada para atualizar para uma data corrente.

Esse projeto utiliza a Api Sidra: https://apisidra.ibge.gov.br/

## Endpoint Correcao
Basta fazer um post para `/correcao/atualizar-valor`, com o seguinte json body de exemplo:
```json
{
  "valorOriginal": 100,
  "anoOriginal": 2020,
  "mesOriginal": 11
}
```

**Curl**:
```curl
curl -X 'POST' \
  'https://localhost:44313/Correcao' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "valorOriginal": 100,
  "anoOriginal": 2020,
  "mesOriginal": 11
}'
```

Retorno esperado da API:
```json
{
  "valorOriginal": 100,
  "valorAtualizado": 110.56,
  "indiceAcumulado": 10.56
}
```
