#!/bin/bash

# Diretórios base
BASE_DIR="./loki"
DATA_DIR="$BASE_DIR/data"
CONFIG_FILE="$BASE_DIR/local-config.yaml"
CHUNKS_DIR="$DATA_DIR/chunks"
INDEX_DIR="$DATA_DIR/index"
CACHE_DIR="$DATA_DIR/cache"
COMPACTOR_DIR="$DATA_DIR/compactor"

# UID que o Loki usa internamente
LOKI_UID=10001

echo "🔍 Verificando estrutura do Loki..."

# Verifica se o arquivo de configuração existe
if [ ! -f "$CONFIG_FILE" ]; then
  echo "❌ Arquivo de configuração não encontrado: $CONFIG_FILE"
  exit 1
fi
echo "✅ Arquivo de configuração encontrado: $CONFIG_FILE"

# Cria os diretórios, se necessário
for dir in "$CHUNKS_DIR" "$INDEX_DIR" "$CACHE_DIR" "$COMPACTOR_DIR"; do
  if [ ! -d "$dir" ]; then
    echo "📁 Criando diretório: $dir"
    mkdir -p "$dir"
  else
    echo "✅ Diretório existe: $dir"
  fi
done

# Corrige permissões
echo "🔧 Ajustando permissões para UID $LOKI_UID em $DATA_DIR"
sudo chown -R "$LOKI_UID:$LOKI_UID" "$DATA_DIR"

# Validação final
echo ""
echo "📂 Estrutura final do diretório $DATA_DIR:"
tree "$DATA_DIR" || ls -lR "$DATA_DIR"

echo ""
echo "✅ Tudo pronto! Agora você pode rodar:"
echo "   docker-compose up -d loki"