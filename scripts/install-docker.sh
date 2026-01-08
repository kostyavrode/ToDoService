#!/bin/bash

set -e

echo "=== Установка Docker ==="

if command -v docker &> /dev/null; then
    echo "Docker уже установлен: $(docker --version)"
    exit 0
fi

if [ -f /etc/os-release ]; then
    . /etc/os-release
    OS=$ID
else
    echo "Не удалось определить ОС"
    exit 1
fi

echo "Обнаружена ОС: $OS"

if [ "$OS" = "ubuntu" ] || [ "$OS" = "debian" ]; then
    echo "Установка Docker для Ubuntu/Debian..."
    
    sudo apt-get update
    sudo apt-get install -y \
        ca-certificates \
        curl \
        gnupg \
        lsb-release
    
    sudo mkdir -p /etc/apt/keyrings
    curl -fsSL https://download.docker.com/linux/$OS/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
    
    echo \
      "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/$OS \
      $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
    
    sudo apt-get update
    sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
    
    sudo usermod -aG docker $USER
    
    echo "Docker установлен успешно!"
    echo "ВАЖНО: Выйдите и войдите снова, чтобы изменения вступили в силу"
    
elif [ "$OS" = "centos" ] || [ "$OS" = "rhel" ] || [ "$OS" = "fedora" ]; then
    echo "Установка Docker для CentOS/RHEL/Fedora..."
    
    sudo yum install -y yum-utils
    sudo yum-config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
    sudo yum install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
    
    sudo systemctl start docker
    sudo systemctl enable docker
    
    sudo usermod -aG docker $USER
    
    echo "Docker установлен успешно!"
    echo "ВАЖНО: Выйдите и войдите снова, чтобы изменения вступили в силу"
else
    echo "Неподдерживаемая ОС: $OS"
    echo "Пожалуйста, установите Docker вручную: https://docs.docker.com/get-docker/"
    exit 1
fi

echo "Проверка установки..."
docker --version
docker compose version
