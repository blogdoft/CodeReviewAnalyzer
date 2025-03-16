#!/bin/sh
# vim:sw=4:ts=4:et

## Every thing down
echo "############# Down services"
cd eng/docker 
docker compose down 
cd ..
cd ..

## Build Containeres
cd eng/docker

echo "############# Building containers"
echo "Solution is auto built inside each image"

docker compose build

## Start everything
echo "############# Starting up"
docker compose up

cd ..
cd ..
