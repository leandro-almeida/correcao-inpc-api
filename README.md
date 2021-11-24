# Correcao INPC Api
Este projeto é apenas uma PoC (proof of concept) de API para corrigir um valor monetário pelo índice INPC, em reais (R$). Desenvolvi rapidamente (cerca de 1h) para chegar no resultado final desejado, dado um valor e uma data passada para atualizar para uma data corrente.

Esse projeto utiliza a Api Sidra: https://apisidra.ibge.gov.br/

## Endpoint Correcao
Basta fazer um post para `/correcao/atualizar-valor`, com o seguinte json body de exemplo:
```json
{
  "valorOriginal": 31.56,
  "anoOriginal": 2021,
  "mesOriginal": 1,
  "anoAtualizacao": 2021,
  "mesAtualizacao": 10
}
```
Esta requisição solicita a atualização monetária do valor original R$31,56 de Janeiro/2021 até Outubro/2021.

**Curl**:
```curl
curl -X 'POST' \
  'https://localhost:44313/Correcao/atualizar-valor' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "valorOriginal": 31.56,
  "anoOriginal": 2021,
  "mesOriginal": 1,
  "anoAtualizacao": 2021,
  "mesAtualizacao": 10
}'
```

Retorno esperado da API:
```json
{
  "valorOriginal": 31.56,
  "valorAtualizado": 33.83460655113834,
  "indiceAcumulado": 7.207245092326819
}
```
