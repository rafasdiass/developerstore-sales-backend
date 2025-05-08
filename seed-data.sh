#!/usr/bin/env bash
set -euo pipefail

API="http://localhost:5211/api"

echo "=== Criando filiais ==="
BR1_LOC=$(curl -si -X POST "$API/branches" \
  -H 'Content-Type: application/json' \
  -d '{"name":"Filial Centro"}' \
  | awk '/^Location: /{print $2}' | tr -d '\r')
BR1_ID=${BR1_LOC##*/}

BR2_LOC=$(curl -si -X POST "$API/branches" \
  -H 'Content-Type: application/json' \
  -d '{"name":"Filial Zona Sul"}' \
  | awk '/^Location: /{print $2}' | tr -d '\r')
BR2_ID=${BR2_LOC##*/}

echo "  Centro : $BR1_ID"
echo "  Zona Sul: $BR2_ID"

echo
echo "=== Criando clientes ==="
C1_LOC=$(curl -si -X POST "$API/customers" \
  -H 'Content-Type: application/json' \
  -d '{"name":"Alice Silva"}' \
  | awk '/^Location: /{print $2}' | tr -d '\r')
C1_ID=${C1_LOC##*/}

C2_LOC=$(curl -si -X POST "$API/customers" \
  -H 'Content-Type: application/json' \
  -d '{"name":"Bruno Costa"}' \
  | awk '/^Location: /{print $2}' | tr -d '\r')
C2_ID=${C2_LOC##*/}

echo "  Alice Silva : $C1_ID"
echo "  Bruno Costa : $C2_ID"

echo
echo "=== Criando vendas ==="
# Venda 1: Alice na Centro, 2 unidades de Produto X a R$12,50
SALE1_LOC=$(curl -si -X POST "$API/sales" \
  -H 'Content-Type: application/json' \
  -d "{
    \"saleDate\": \"$(date -u +%Y-%m-%dT%H:%M:%SZ)\",
    \"customerId\": \"$C1_ID\",
    \"customerName\": \"Alice Silva\",
    \"branchId\": \"$BR1_ID\",
    \"branchName\": \"Filial Centro\",
    \"items\": [
      {
        \"productId\": \"11111111-1111-1111-1111-111111111111\",
        \"productName\": \"Produto X\",
        \"quantity\": 2,
        \"unitPrice\": 12.50
      }
    ]
  }" \
  | awk '/^Location: /{print $2}' | tr -d '\r')
SALE1_ID=${SALE1_LOC##*/}

echo "  Venda 1 (Alice) : $SALE1_ID"

# Venda 2: Bruno na Zona Sul, 5 unidades de Produto Y a R$8,30
SALE2_LOC=$(curl -si -X POST "$API/sales" \
  -H 'Content-Type: application/json' \
  -d "{
    \"saleDate\": \"$(date -u +%Y-%m-%dT%H:%M:%SZ)\",
    \"customerId\": \"$C2_ID\",
    \"customerName\": \"Bruno Costa\",
    \"branchId\": \"$BR2_ID\",
    \"branchName\": \"Filial Zona Sul\",
    \"items\": [
      {
        \"productId\": \"22222222-2222-2222-2222-222222222222\",
        \"productName\": \"Produto Y\",
        \"quantity\": 5,
        \"unitPrice\": 8.30
      }
    ]
  }" \
  | awk '/^Location: /{print $2}' | tr -d '\r')
SALE2_ID=${SALE2_LOC##*/}

echo "  Venda 2 (Bruno): $SALE2_ID"

echo
echo "=== Seed concluído! ==="
