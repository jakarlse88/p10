#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update
RUN ["apt-get","install","-y","vim"]
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Abarnathy.DemographicsAPI/src/Abarnathy.DemographicsAPI.csproj", "Abarnathy.DemographicsAPI/"]
RUN dotnet restore "Abarnathy.DemographicsAPI/Abarnathy.DemographicsAPI.csproj"
COPY . .
WORKDIR "/src/Abarnathy.DemographicsAPI"
RUN dotnet build "Abarnathy.DemographicsAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abarnathy.DemographicsAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Abarnathy.DemographicsAPI/src/. .
COPY Abarnathy.DemographicsAPI/src/wait-for-it.sh ./wait-for-it.sh
RUN chmod +x ./wait-for-it.sh
RUN ./fix-line-endings.sh
ENTRYPOINT ["./wait-for-it.sh", "sql_server:1434", "-t", "40", "--", "dotnet", "Abarnathy.DemographicsAPI.dll"]