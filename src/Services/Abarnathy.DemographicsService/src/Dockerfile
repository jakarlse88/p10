#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update
RUN ["apt-get","install","-y","dos2unix"]
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Abarnathy.DemographicsService/src/Abarnathy.DemographicsService.csproj", "Services/Abarnathy.DemographicsService/"]
RUN dotnet restore "Services/Abarnathy.DemographicsService/Abarnathy.DemographicsService.csproj"
COPY . .
WORKDIR "/src/Services/Abarnathy.DemographicsService"
RUN dotnet build "Abarnathy.DemographicsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abarnathy.DemographicsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Services/Abarnathy.DemographicsService/src/. .
COPY Services/Abarnathy.DemographicsService/src/wait-for-it.sh ./wait-for-it.sh
RUN dos2unix ./wait-for-it.sh && apt-get --purge remove -y dos2unix && rm -rf /var/lib/apt/lists/*
RUN chmod +x ./wait-for-it.sh
ENTRYPOINT ["./wait-for-it.sh", "sql_server:1434", "-t", "40", "--", "dotnet", "Abarnathy.DemographicsService.dll"]