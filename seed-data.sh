#!/usr/bin/env bash
set -euo pipefail

API="http://localhost:5211/api"
CURL_OPTS=(-sSf -H "Content-Type: application/json")

echo
echo "=== 1) Seed de Filiais ==="
BR1_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/branches" -d '{"name":"Filial Centro"}' | jq -r '.id')
BR2_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/branches" -d '{"name":"Filial Zona Sul"}' | jq -r '.id')
BR3_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/branches" -d '{"name":"Filial Norte"}' | jq -r '.id')

echo "  • Filial Centro  -> $BR1_ID"
echo "  • Filial Zona Sul-> $BR2_ID"
echo "  • Filial Norte   -> $BR3_ID"

echo
echo "=== 2) Seed de Clientes ==="
C1_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/customers" -d '{"name":"Alice Silva", "email":"alice.silva@example.com"}' | jq -r '.id')
C2_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/customers" -d '{"name":"Bruno Costa", "email":"bruno.costa@example.com"}' | jq -r '.id')
C3_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/customers" -d '{"name":"Carla Souza", "email":"carla.souza@example.com"}' | jq -r '.id')
C4_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/customers" -d '{"name":"Daniel Santos", "email":"daniel.santos@example.com"}' | jq -r '.id')

echo "  • Alice Silva    -> $C1_ID"
echo "  • Bruno Costa    -> $C2_ID"
echo "  • Carla Souza    -> $C3_ID"
echo "  • Daniel Santos  -> $C4_ID"

echo
echo "=== 3) Seed de Produtos ==="
P1_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/products" -d '{"name":"Produto X","price":10.00}' | jq -r '.id')
P2_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/products" -d '{"name":"Produto Y","price":15.50}' | jq -r '.id')
P3_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/products" -d '{"name":"Produto Z","price":7.25}' | jq -r '.id')

echo "  • Produto X -> $P1_ID (R\$10,00)"
echo "  • Produto Y -> $P2_ID (R\$15,50)"
echo "  • Produto Z -> $P3_ID (R\$7,25)"

echo
echo "=== 4) Seed de Vendas ==="
SALE1_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/sales" -d "{
  \"saleNumber\": \"\",
  \"saleDate\": \"$(date -u +%Y-%m-%dT%H:%M:%SZ)\",
  \"customerId\": \"$C1_ID\",
  \"customerName\": \"Alice Silva\",
  \"branchId\": \"$BR1_ID\",
  \"branchName\": \"Filial Centro\",
  \"items\": [
    {\"productId\":\"$P1_ID\",\"productName\":\"Produto X\",\"quantity\":2,\"unitPrice\":10.00}
  ]
}" | jq -r '.id')
echo "  • Venda 1 (Alice) -> $SALE1_ID"

SALE2_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/sales" -d "{
  \"saleNumber\": \"\",
  \"saleDate\": \"$(date -u +%Y-%m-%dT%H:%M:%SZ)\",
  \"customerId\": \"$C2_ID\",
  \"customerName\": \"Bruno Costa\",
  \"branchId\": \"$BR2_ID\",
  \"branchName\": \"Filial Zona Sul\",
  \"items\": [
    {\"productId\":\"$P2_ID\",\"productName\":\"Produto Y\",\"quantity\":1,\"unitPrice\":15.50},
    {\"productId\":\"$P3_ID\",\"productName\":\"Produto Z\",\"quantity\":3,\"unitPrice\":7.25}
  ]
}" | jq -r '.id')
echo "  • Venda 2 (Bruno) -> $SALE2_ID"

SALE3_ID=$(curl "${CURL_OPTS[@]}" -X POST "$API/sales" -d "{
  \"saleNumber\": \"\",
  \"saleDate\": \"$(date -u +%Y-%m-%dT%H:%M:%SZ)\",
  \"customerId\": \"$C3_ID\",
  \"customerName\": \"Carla Souza\",
  \"branchId\": \"$BR3_ID\",
  \"branchName\": \"Filial Norte\",
  \"items\": [
    {\"productId\":\"$P3_ID\",\"productName\":\"Produto Z\",\"quantity\":5,\"unitPrice\":7.25}
  ]
}" | jq -r '.id')
echo "  • Venda 3 (Carla) -> $SALE3_ID"

echo
echo "=== 5) Atualização da Venda 2 (PATCH) ==="
curl "${CURL_OPTS[@]}" -X PATCH "$API/sales/$SALE2_ID" -d "{
  \"saleDate\": \"$(date -u +%Y-%m-%dT%H:%M:%SZ)\",
  \"customerId\": \"$C2_ID\",
  \"customerName\": \"Bruno Costa\",
  \"branchId\": \"$BR2_ID\",
  \"branchName\": \"Filial Zona Sul\",
  \"items\": [
    {\"productId\":\"$P2_ID\",\"productName\":\"Produto Y\",\"quantity\":10,\"unitPrice\":15.50},
    {\"productId\":\"$P3_ID\",\"productName\":\"Produto Z\",\"quantity\":5,\"unitPrice\":7.25}
  ]
}"

echo "  • Venda 2 atualizada com novas quantidades."

echo
echo "=== 6) Exclusão da Venda 3 (DELETE) ==="
curl "${CURL_OPTS[@]}" -X DELETE "$API/sales/$SALE3_ID"
echo "  • Venda 3 (Carla) excluída com sucesso."

echo
echo "=== ✅ Seed + Update + Delete finalizado com sucesso! ==="
