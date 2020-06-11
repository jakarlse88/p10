#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Client/Abarnathy.BlazorClient/Server/Abarnathy.BlazorClient.Server.csproj", "Client/Abarnathy.BlazorClient/Server/"]
COPY ["Client/Abarnathy.BlazorClient/Client/Abarnathy.BlazorClient.Client.csproj", "Client/Abarnathy.BlazorClient/Client/"]
RUN dotnet restore "Client/Abarnathy.BlazorClient/Server/Abarnathy.BlazorClient.Server.csproj"
COPY . .
WORKDIR "/src/Client/Abarnathy.BlazorClient/Server"
RUN dotnet build "Abarnathy.BlazorClient.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abarnathy.BlazorClient.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /app/publish/wwwroot .
ENTRYPOINT ["dotnet", "Abarnathy.BlazorClient.Server.dll"]