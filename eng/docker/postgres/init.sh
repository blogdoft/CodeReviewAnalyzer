#!/bin/bash
set -e



echo "Aguardando PostgreSQL iniciar..."
until pg_isready -h "localhost" -U "$POSTGRES_USER" -d "$POSTGRES_DB"; do
  sleep 2
  echo "Ainda aguardando PostgreSQL..."
done


for filename in *.sql; do
  echo "Replacing $filename"
  envsubst < $filename > tmp.sql
  echo "PostgreSQL pronto! Executando script de inicialização..."
  PGPASSWORD=$POSTGRES_PASSWORD psql -h "localhost" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f ./tmp.sql
  echo "Script de inicialização concluído!"
done;
