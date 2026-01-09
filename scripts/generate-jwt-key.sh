#!/bin/bash

echo "=== Генератор JWT Secret Key ==="
echo ""

if command -v openssl &> /dev/null; then
    echo "Используя OpenSSL..."
    JWT_KEY=$(openssl rand -base64 32)
    echo "Сгенерированный JWT_SECRET_KEY:"
    echo "$JWT_KEY"
    echo ""
    echo "Длина: ${#JWT_KEY} символов"
elif command -v python3 &> /dev/null; then
    echo "Используя Python..."
    JWT_KEY=$(python3 -c "import secrets; print(secrets.token_urlsafe(32))")
    echo "Сгенерированный JWT_SECRET_KEY:"
    echo "$JWT_KEY"
    echo ""
    echo "Длина: ${#JWT_KEY} символов"
else
    echo "OpenSSL и Python3 не найдены. Используйте один из способов:"
    echo ""
    echo "1. Установите openssl: sudo apt install openssl"
    echo "2. Установите python3: sudo apt install python3"
    echo "3. Или сгенерируйте вручную строку минимум 32 символа"
    exit 1
fi

echo ""
echo "Скопируйте этот ключ и вставьте в .env файл:"
echo "JWT_SECRET_KEY=$JWT_KEY"
