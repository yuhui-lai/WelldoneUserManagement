# 基礎階段
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# 構建參數
ARG BUILD_CONFIGURATION=Release
ARG DOTNET_ENV=Production
ENV ASPNETCORE_ENVIRONMENT=$DOTNET_ENV

# .NET 構建階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY ["WUM.Server/WUM.Server.csproj", "WUM.Server/"]
COPY ["WUM.Lib/WUM.Lib.csproj", "WUM.Lib/"]
RUN dotnet restore "./WUM.Server/WUM.Server.csproj"
COPY . .
WORKDIR "/src/WUM.Server"
RUN dotnet build "./WUM.Server.csproj" -c Release -o /app/build

# 發佈階段
FROM build AS publish
ARG BUILD_CONFIGURATION
RUN dotnet publish "./WUM.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Node.js 構建階段
FROM node:alpine AS npm_base
WORKDIR /app
COPY wum.client/package*.json ./
RUN npm install
ARG NODE_ENV
ENV NODE_ENV=$NODE_ENV
COPY wum.client/. .
RUN if [ "$NODE_ENV" = "production" ]; then \
    echo "Running production build" && npm run build; \
    elif [ "$NODE_ENV" = "staging" ]; then \
    echo "Running staging build" && npm run staging; \
    else \
    echo "Running development build" && npm run dev; \
    fi

# 最終階段
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=npm_base /app/dist ./wwwroot
ENTRYPOINT ["dotnet", "WUM.Server.dll"]


# docker build --build-arg DOTNET_ENV=Staging --build-arg NODE_ENV=staging -t wum:staging .
# docker build --build-arg DOTNET_ENV=Production --build-arg NODE_ENV=production -t wum:latest .
