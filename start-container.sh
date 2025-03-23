#!/bin/sh
# vim:sw=4:ts=4:et

ROOT_DIR=$(pwd)

## Configurar Keycloak local
echo "############# Verificando certificados do Keycloak"

CERT_DIR="./eng/docker/keycloak/certs"
KEY_FILE="$CERT_DIR/keycloak.key"
CRT_FILE="$CERT_DIR/keycloak.crt"
P12_FILE="$CERT_DIR/keycloak.p12"

mkdir -p "$CERT_DIR"

# Verifica se todos os arquivos já existem
if [ -f "$KEY_FILE" ] && [ -f "$CRT_FILE" ] && [ -f "$P12_FILE" ]; then
    echo "✔ Certificados já existem. Nenhuma ação necessária."
else
    cd "$CERT_DIR"

    echo "🔐 Gerando novos certificados para o Keycloak..."

    openssl genrsa -out keycloak.key 2048

    openssl req -new -x509 -key keycloak.key -out keycloak.crt -days 365 -config keycloak.cnf

    openssl pkcs12 -export -in keycloak.crt -inkey keycloak.key -out keycloak.p12 -name keycloak -password pass:changeit

    echo "✅ Certificados gerados com sucesso."

    echo ""
    echo "Você precisa configurar o seu arquivo hosts, adicionando nele a linha:"
    echo "127.0.0.1 keycloak    localhost"

fi

# Retornar ao diretório raiz do projeto
cd $ROOT_DIR


## Every thing down
echo "############# Down services"
cd eng/docker 
docker compose down 
cd $ROOT_DIR

## Build Containeres
cd eng/docker

echo "############# Building containers"
echo "Solution is auto built inside each image"

docker compose build

## Start everything
echo "############# Starting up"
docker compose up -d

cd $ROOT_DIR
