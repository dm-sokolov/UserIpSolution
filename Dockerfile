# Stage: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build

# Копируем решение и проекты
COPY *.sln ./
COPY src/UserIpService.Core/*.csproj ./UserIpService.Core/
COPY src/UserIpService.Infrastructure/*.csproj ./UserIpService.Infrastructure/
COPY src/UserIpService.Api/*.csproj ./UserIpService.Api/

# Восстанавливаем зависимости
RUN dotnet restore UserIpService.Api/UserIpService.Api.csproj

# Копируем остальной код
COPY src/ ./ 

# Публикуем
WORKDIR /build/UserIpService.Api
RUN dotnet publish -c Release -o /app/publish

# Stage: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "UserIpService.Api.dll"]