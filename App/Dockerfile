FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY "App.csproj" .
RUN dotnet restore "App.csproj"
COPY . .
RUN dotnet build "App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

USER root
RUN mkdir -p /endpoints
RUN chown $APP_UID /endpoints
USER $APP_UID

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.dll", "-d /endpoints"]
