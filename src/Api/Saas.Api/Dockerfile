# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["src/Api/Saas.Api/Saas.Api.csproj", "src/Api/Saas.Api/"]
COPY ["aspire/SaasApi.ServiceDefaults/SaasApi.ServiceDefaults.csproj", "aspire/SaasApi.ServiceDefaults/"]
COPY ["src/Modules/Events/Saas.Modules.Events.Api/Saas.Modules.Events.Api.csproj", "src/Modules/Events/Saas.Modules.Events.Api/"]
RUN dotnet restore "./src/Api/Sass.Api/Saas.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/Saas.Api"
RUN dotnet build "./Saas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Saas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Saas.Api.dll"]