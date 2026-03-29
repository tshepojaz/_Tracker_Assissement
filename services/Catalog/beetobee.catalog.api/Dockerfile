FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5001

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["services/Catalog/beetobee.catalog.api/beetobee.catalog.api.csproj", "services/Catalog/beetobee.catalog.api/"]
RUN dotnet restore "services/Catalog/beetobee.catalog.api/beetobee.catalog.api.csproj"
COPY . .
WORKDIR "/src/services/Catalog/beetobee.catalog.api"
RUN dotnet build "beetobee.catalog.api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "beetobee.catalog.api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "beetobee.catalog.api.dll"]
