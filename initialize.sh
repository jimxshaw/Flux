#!/bin/bash

# Use this bash script to tear down and then spin up all docker containers.
echo "Stopping and removing existing containers and volumes..."
docker compose down -v

echo "Rebuilding containers without cache..."
docker compose build --no-cache

echo "Starting containers in detached mode..."
docker compose up -d
