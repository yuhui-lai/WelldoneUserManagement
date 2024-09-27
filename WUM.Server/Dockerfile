# 請參閱 https://aka.ms/customizecontainer 了解如何自訂您的偵錯容器，以及 Visual Studio 如何使用此 Dockerfile 來組建您的映像，以加快偵錯速度。

# 此階段用於以快速模式從 VS 執行時 (偵錯設定的預設值)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG BUILD_CONFIGURATION=Release
ARG ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT

# 此階段是用來組建服務專案
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WUM.Server/WUM.Server.csproj", "WUM.Server/"]
COPY ["WUM.Lib/WUM.Lib.csproj", "WUM.Lib/"]
RUN dotnet restore "./WUM.Server/WUM.Server.csproj"
COPY . .
WORKDIR "/src/WUM.Server"
RUN dotnet build "./WUM.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此階段可用來發佈要複製到最終階段的服務專案
FROM build AS publish
RUN dotnet publish "./WUM.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此階段用於生產環境，或以一般模式從 VS 執行時 (未使用偵錯設定時的預設值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WUM.Server.dll"]

# docker build --build-arg BUILD_CONFIGURATION=Debug --build-arg ENVIRONMENT=Development -t wum:dev .
# docker build --build-arg BUILD_CONFIGURATION=Release --build-arg ENVIRONMENT=Production -t wum:latest .
# docker build --build-arg BUILD_CONFIGURATION=Release --build-arg ENVIRONMENT=Staging -t wum:staging .
