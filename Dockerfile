FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["src/Treinu.Api/Treinu.Api.csproj", "src/Treinu.Api/"]
COPY ["src/Treinu.Application/Treinu.Application.csproj", "src/Treinu.Application/"]
COPY ["src/Treinu.Domain/Treinu.Domain.csproj", "src/Treinu.Domain/"]
COPY ["src/Treinu.Infrastructure/Treinu.Infrastructure.csproj", "src/Treinu.Infrastructure/"]

RUN dotnet restore "./src/Treinu.Api/Treinu.Api.csproj"

# Copy everything else and build
COPY src/ src/
WORKDIR "/src/src/Treinu.Api"
RUN dotnet build "./Treinu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Treinu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Treinu.Api.dll"]
