# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PersonalFinanceManager.API/PersonalFinanceManager.API.csproj", "PersonalFinanceManager.API/"]
COPY ["PersonalFinanceManager.Application/PersonalFinanceManager.Application.csproj", "PersonalFinanceManager.Application/"]
COPY ["PersonalFinanceManager.Application.Contracts/PersonalFinanceManager.Application.Contracts.csproj", "PersonalFinanceManager.Application.Contracts/"]
COPY ["PersonalFinanceManager.Domain/PersonalFinanceManager.Domain.csproj", "PersonalFinanceManager.Domain/"]
COPY ["PersonalFinanceManager.Shared/PersonalFinanceManager.Shared.csproj", "PersonalFinanceManager.Shared/"]
COPY ["PersonalFinanceManager.Infrastructure/PersonalFinanceManager.Infrastructure.csproj", "PersonalFinanceManager.Infrastructure/"]
COPY ["PersonalFinanceManager.Presentation/PersonalFinanceManager.Presentation.csproj", "PersonalFinanceManager.Presentation/"]
RUN dotnet restore "./PersonalFinanceManager.API/PersonalFinanceManager.API.csproj"
COPY . .
WORKDIR "/src/PersonalFinanceManager.API"
RUN dotnet build "./PersonalFinanceManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PersonalFinanceManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalFinanceManager.API.dll"]