﻿# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY . .

WORKDIR /src/WebService
RUN dotnet publish -c release -o /app
    
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WebService.dll"]

