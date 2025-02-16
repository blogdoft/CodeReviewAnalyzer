#!/bin/sh
# vim:sw=4:ts=4:et

## Every thing down
echo "############# Down services"
cd eng/docker 
docker compose down 
cd ..
cd ..

## Build backend
echo "############# Building backend"
cd backend
dotnet publish ./src/CodeReviewAnalyzer.Api -c Release -o ./app
cd ..

# ## Build frontend
echo "############# Building frontend"
cd frontend
ng build
cd ..

## Build Containeres
echo "############# Building containers"
cd eng/docker
docker compose build

## Start everything
echo "############# starting up"
docker compose up

cd ..
cd ..
