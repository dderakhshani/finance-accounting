FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
WORKDIR "/src/Microservices/Identity/Eefa.Identity.WebApi"
#RUN dotnet build "Eefa.Identity.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eefa.Identity.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eefa.Identity.WebApi.dll"]