#!/bin/bash

set -e

echo "=== Настройка проекта ToDo API ==="

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

cd "$PROJECT_DIR"

generate_jwt_key() {
    if command -v openssl &> /dev/null; then
        openssl rand -base64 32
    elif command -v python3 &> /dev/null; then
        python3 -c "import secrets; print(secrets.token_urlsafe(32))"
    else
        echo ""
        return 1
    fi
}

generate_password() {
    if command -v openssl &> /dev/null; then
        openssl rand -base64 12 | tr -d "=+/" | head -c 16
    elif command -v python3 &> /dev/null; then
        python3 -c "import secrets, string; chars = string.ascii_letters + string.digits + '!@#\$%^&*'; print(''.join(secrets.choice(chars) for _ in range(16)))"
    else
        echo ""
        return 1
    fi
}

if [ ! -f ".env" ]; then
    if [ ! -f ".env.example" ]; then
        echo "Создание .env.example файла..."
        cat > .env.example << 'EOF'
POSTGRES_DB=todo_db
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_secure_password_here

JWT_SECRET_KEY=your_super_secret_key_minimum_32_characters_long_and_unique
JWT_ISSUER=todo-kostya.online
JWT_AUDIENCE=todo-kostya.online
JWT_EXPIRY_MINUTES=1440
EOF
    fi
    echo "Создание .env файла из примера..."
    cp .env.example .env
    echo ""
    echo "=== Настройка секретных ключей ==="
    echo ""
    
    POSTGRES_PASSWORD=""
    JWT_SECRET_KEY=""
    
    echo "1. Генерация POSTGRES_PASSWORD (пароль для базы данных)"
    read -p "Сгенерировать автоматически? (y/n): " gen_password
    if [ "$gen_password" = "y" ] || [ "$gen_password" = "Y" ]; then
        POSTGRES_PASSWORD=$(generate_password)
        if [ -z "$POSTGRES_PASSWORD" ]; then
            echo "⚠️  Не удалось сгенерировать автоматически. Введите пароль вручную:"
            read -s POSTGRES_PASSWORD
        else
            echo "✅ Сгенерирован пароль: $POSTGRES_PASSWORD"
        fi
    else
        echo "Введите пароль для PostgreSQL:"
        read -s POSTGRES_PASSWORD
    fi
    
    echo ""
    echo "2. Генерация JWT_SECRET_KEY (секретный ключ для подписи JWT токенов)"
    echo "   ⚠️  ВАЖНО: Это НЕ токен пользователя! Это ключ для шифрования всех токенов."
    read -p "Сгенерировать автоматически? (y/n): " gen_jwt
    if [ "$gen_jwt" = "y" ] || [ "$gen_jwt" = "Y" ]; then
        JWT_SECRET_KEY=$(generate_jwt_key)
        if [ -z "$JWT_SECRET_KEY" ]; then
            echo "⚠️  OpenSSL и Python3 не найдены. Введите ключ вручную (минимум 32 символа):"
            read -s JWT_SECRET_KEY
        else
            echo "✅ Сгенерирован JWT_SECRET_KEY (длина: ${#JWT_SECRET_KEY} символов)"
        fi
    else
        echo "Введите JWT_SECRET_KEY (минимум 32 символа):"
        read -s JWT_SECRET_KEY
    fi
    
    echo ""
    echo "Обновление .env файла..."
    
    if [ -n "$POSTGRES_PASSWORD" ]; then
        awk -v pwd="$POSTGRES_PASSWORD" 'BEGIN {FS=OFS="="} /^POSTGRES_PASSWORD=/ {$2=pwd} {print}' .env > .env.tmp && mv .env.tmp .env
    fi
    
    if [ -n "$JWT_SECRET_KEY" ]; then
        awk -v key="$JWT_SECRET_KEY" 'BEGIN {FS=OFS="="} /^JWT_SECRET_KEY=/ {$2=key} {print}' .env > .env.tmp && mv .env.tmp .env
    fi
    
    echo "✅ .env файл обновлен"
    echo ""
    echo "Сгенерированные ключи сохранены в .env файле."
    echo "Для просмотра: cat .env"
    echo ""
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
