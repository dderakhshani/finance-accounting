
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY Microservices/Admin/Eefa.Admin.WebApi/Eefa.Admin.WebApi.csproj Microservices/Admin/Eefa.Admin.WebApi/
RUN dotnet restore Microservices/Admin/Eefa.Admin.WebApi/Eefa.Admin.WebApi.csproj

COPY . .
WORKDIR /src/Microservices/Admin/Eefa.Admin.WebApi
RUN dotnet build Eefa.Admin.WebApi.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Eefa.Admin.WebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Eefa.Admin.WebApi.dll"]