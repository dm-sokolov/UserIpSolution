# UserIpSolution

Сервис для работы с пользователями и их IP адресами.

Сервис позволяет:
●	фиксирует данные пользователя и его IP в БД.
●	найти пользователей по начальной или полной части IP адреса (например, если в качестве строки поиска указать “31.214”, а у пользователя 1234567 зафиксированы ранее следующие IP ["31.214.157.141", "62.4.36.194"], то метод сервиса вернет список, в котором помимо прочих подходящих пользователей будет и 1234567) 
●	найти все накопленные IP адреса пользователя 
●	найти время и IP адрес последнего подключения пользователя
●	найти время последнего подключения пользователя по любому из его IP


# Сборка и запуск

Запуск в контейнерах и БД и API
docker-compose -f docker-compose.yml up -d --build

Запуск в контейнере только БД
docker-compose -f docker-compose.postgres.yml up -d --build

Применение миграций
dotnet ef migrations add InitialCreate --project .\src\UserIpService.Infrastructure --startup-project .\src\UserIpService.Api
dotnet ef database update --project .\src\UserIpService.Infrastructure --startup-project .\src\UserIpService.Api

