#!/bin/bash
set -e

echo "Aguardando PostgreSQL iniciar..."
until pg_isready -h "localhost" -U "$POSTGRES_USER" -d "$POSTGRES_DB"; do
  sleep 2
  echo "Ainda aguardando PostgreSQL..."
done

# Criando banco de dados

# criando banco de dados do keycloak
EXISTS=$(PGPASSWORD=$POSTGRES_PASSWORD psql -h "localhost" -U "$POSTGRES_USER" -tAc "SELECT 1 FROM pg_database WHERE datname = 'kc-db';")

if [ "$EXISTS" != "1" ]; then
  echo "🛠 Criando banco de dados 'kc-db'..."
  PGPASSWORD=$POSTGRES_PASSWORD psql -h "localhost" -U "$POSTGRES_USER" -tAc "create database \"kc-db\";"
else
  echo "✔ Banco de dados 'kc-db' já existe. Nenhuma ação necessária."
fi

# criando banco de dados da aplicação
EXISTS=$(PGPASSWORD=$POSTGRES_PASSWORD psql -h "localhost" -U "$POSTGRES_USER" -tAc "SELECT 1 FROM pg_database WHERE datname = '$POSTGRES_DB';")

if [ "$EXISTS" != "1" ]; then
  echo "🛠 Criando banco de dados 'kc-db'..."
  PGPASSWORD=$POSTGRES_PASSWORD psql -h "localhost" -U "$POSTGRES_USER" -tAc "create database \"$POSTGRES_DB\";"
else
  echo "✔ Banco de dados '$POSTGRES_DB' já existe. Nenhuma ação necessária."
fi


for filename in /init-sql/*.sql; do
  echo "Replacing $filename"
  envsubst < $filename > tmp.sql
  echo "PostgreSQL pronto! Executando script de inicialização..."
  PGPASSWORD=$POSTGRES_PASSWORD psql -h "localhost" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f ./tmp.sql 
  echo "Script de inicialização concluído!"
done;
