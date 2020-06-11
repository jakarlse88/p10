FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Abarnathy.HistoryService/src/Abarnathy.HistoryService.csproj", "Services/Abarnathy.HistoryService/"]
RUN dotnet restore "Services/Abarnathy.HistoryService/Abarnathy.HistoryService.csproj"
COPY . .
WORKDIR "/src/Services/Abarnathy.HistoryService"
RUN dotnet build "Abarnathy.HistoryService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abarnathy.HistoryService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Services/Abarnathy.HistoryService/src/. .
ENTRYPOINT ["dotnet", "Abarnathy.HistoryService.dll"]