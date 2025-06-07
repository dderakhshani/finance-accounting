
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["Microservices/Admin/Eefa.Admin.WebApi", "Microservices/Admin/Eefa.Admin.WebApi/"]
COPY ["Microservices/Admin/Eefa.Admin.Data", "Microservices/Admin/Eefa.Admin.Data/"]
COPY ["Microservices/Admin/Eefa.Admin.Infrastructure", "Microservices/Admin/Eefa.Admin.Infrastructure/"]
COPY ["Microservices/Admin/Eefa.Admin.Application", "Microservices/Admin/Eefa.Admin.Application/"]
COPY ["./Core", "Core"]

RUN dotnet restore "Microservices/Admin/Eefa.Admin.WebApi/Eefa.Admin.WebApi.csproj"


WORKDIR "/src/Microservices/Admin/Eefa.Admin.WebApi"
RUN dotnet build "Eefa.Admin.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eefa.Admin.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ./Microservices/Admin/appsettings.Production.json ./appsettings.json

ENTRYPOINT ["dotnet", "Eefa.Admin.WebApi.dll"]
