# Build — contexto é a raiz do repositório
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/ .
RUN dotnet publish services/NotificationService/NotificationService.Worker/NotificationService.Worker.csproj -c Release -o /app/publish

# Runtime — sem SDK, sem ASP.NET (Worker usa runtime puro)
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Worker.dll"]