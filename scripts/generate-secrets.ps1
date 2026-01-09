Write-Host "=== Генератор секретных ключей ===" -ForegroundColor Cyan
Write-Host ""

Write-Host "1. Генерация JWT_SECRET_KEY (64 символа):" -ForegroundColor Yellow
$jwtKey = [Convert]::ToBase64String((1..48 | ForEach-Object { Get-Random -Minimum 0 -Maximum 256 }))
Write-Host "JWT_SECRET_KEY=$jwtKey" -ForegroundColor Green
Write-Host ""

Write-Host "2. Генерация POSTGRES_PASSWORD (16 символов):" -ForegroundColor Yellow
$password = -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 16 | ForEach-Object {[char]$_})
$special = @('!','@','#','$','%','^','&','*')
$password = $password.Insert((Get-Random -Maximum $password.Length), ($special | Get-Random))
Write-Host "POSTGRES_PASSWORD=$password" -ForegroundColor Green
Write-Host ""

Write-Host "Скопируйте эти значения в ваш .env файл!" -ForegroundColor Cyan
