FROM mcr.microsoft.com/dotnet/aspnet:5.0-windowsservercore-ltsc2019 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-windowsservercore-ltsc2019 AS build
WORKDIR /src

# Copy and restore all projects
COPY ["Microservices/Accounting/Eefa.Accounting.WebApi", "Microservices/Accounting/Eefa.Accounting.WebApi/"]
COPY ["Microservices/Accounting/Eefa.Accounting.Data", "Microservices/Accounting/Eefa.Accounting.Data/"]
COPY ["Microservices/Accounting/Eefa.Accounting.Infrastructure", "Microservices/Accounting/Eefa.Accounting.Infrastructure/"]
COPY ["Microservices/Accounting/Eefa.Accounting.Application", "Microservices/Accounting/Eefa.Accounting.Application/"]
COPY ["Microservices/Accounting/TaxCollectData.Library", "Microservices/Accounting/TaxCollectData.Library/"]
COPY ["./Core", "Core"]

# Restore dependencies
RUN dotnet restore "Microservices/Accounting/Eefa.Accounting.WebApi/Eefa.Accounting.WebApi.csproj"


# Build the application
WORKDIR "/src/Microservices/Accounting/Eefa.Accounting.WebApi"
RUN dotnet build "Eefa.Accounting.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eefa.Accounting.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the production appsettings.json
COPY ./Microservices/Accounting/appsettings.Production.json ./appsettings.json

ENTRYPOINT ["dotnet", "Eefa.Accounting.WebApi.dll"]
