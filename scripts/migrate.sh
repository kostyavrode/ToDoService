#!/bin/bash

set -e

echo "=== Применение миграций БД ==="

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

cd "$PROJECT_DIR"

if [ ! -f ".env" ]; then
    echo "Ошибка: .env файл не найден"
    exit 1
fi

echo "Проверка запущенных контейнеров..."
if ! docker compose ps | grep -q "todo-postgres.*Up"; then
    echo "PostgreSQL контейнер не запущен. Запускаю..."
    docker compose up -d postgres
    echo "Ожидание готовности PostgreSQL..."
    sleep 10
fi

echo "Применение миграций..."
docker compose exec -T postgres psql -U postgres -d todo_db -c "SELECT 1;" > /dev/null 2>&1 || {
    echo "Создание базы данных..."
    docker compose exec -T postgres psql -U postgres -c "CREATE DATABASE todo_db;" || true
}

echo "Миграции будут применены при первом запуске API контейнера"
echo "Или используйте dotnet ef tools внутри контейнера API"
