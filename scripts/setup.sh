#!/bin/bash

set -e

echo "=== Настройка проекта ToDo API ==="

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

cd "$PROJECT_DIR"

if [ ! -f ".env" ]; then
    echo "Создание .env файла из примера..."
    cp .env.example .env
    echo ""
    echo "⚠️  ВАЖНО: Отредактируйте файл .env и установите:"
    echo "   - POSTGRES_PASSWORD (надежный пароль)"
    echo "   - JWT_SECRET_KEY (минимум 32 символа)"
    echo ""
    echo "Файл: $PROJECT_DIR/.env"
    echo ""
    read -p "Нажмите Enter после редактирования .env файла..."
fi

if [ ! -f ".env" ]; then
    echo "Ошибка: .env файл не найден"
    exit 1
fi

source .env

if [ -z "$POSTGRES_PASSWORD" ] || [ "$POSTGRES_PASSWORD" = "your_secure_password_here" ]; then
    echo "Ошибка: Установите POSTGRES_PASSWORD в .env файле"
    exit 1
fi

if [ -z "$JWT_SECRET_KEY" ] || [ "$JWT_SECRET_KEY" = "your_super_secret_key_minimum_32_characters_long_and_unique" ]; then
    echo "Ошибка: Установите JWT_SECRET_KEY в .env файле (минимум 32 символа)"
    exit 1
fi

if [ ${#JWT_SECRET_KEY} -lt 32 ]; then
    echo "Ошибка: JWT_SECRET_KEY должен быть минимум 32 символа"
    exit 1
fi

echo "Проверка Docker..."
if ! command -v docker &> /dev/null; then
    echo "Docker не установлен. Запустите: bash scripts/install-docker.sh"
    exit 1
fi

echo "Сборка Docker образа..."
docker compose build

echo ""
echo "=== Настройка завершена! ==="
echo ""
echo "Для запуска выполните:"
echo "  docker compose up -d"
echo ""
echo "Для просмотра логов:"
echo "  docker compose logs -f"
echo ""
echo "Для остановки:"
echo "  docker compose down"
