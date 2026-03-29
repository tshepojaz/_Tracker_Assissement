FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

ENV ASPNETCORE_URLS=http://+:5002

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["services/Order/beetobee.order.api/beetobee.order.api.csproj", "services/Order/beetobee.order.api/"]
RUN dotnet restore "services/Order/beetobee.order.api/beetobee.order.api.csproj"
COPY . .
WORKDIR "/src/services/Order/beetobee.order.api"
RUN dotnet build "beetobee.order.api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "beetobee.order.api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "beetobee.order.api.dll"]
